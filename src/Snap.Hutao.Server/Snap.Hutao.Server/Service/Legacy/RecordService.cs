﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Storage;
using Snap.Hutao.Server.Extension;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity.SpiralAbyss;
using Snap.Hutao.Server.Model.Upload;
using Snap.Hutao.Server.Service.GachaLog;
using Snap.Hutao.Server.Service.Legacy.PizzaHelper;
using Snap.Hutao.Server.Service.Ranking;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Snap.Hutao.Server.Service.Legacy;

public sealed class RecordService
{
    private const int GachaLogExtendDays = 3;

    private static readonly ConcurrentDictionary<string, UploadToken> UploadingUids = new();

    private readonly IMemoryCache memoryCache;
    private readonly AppDbContext appDbContext;
    private readonly IRankService rankService;
    private readonly ExpireService expireService;
    private readonly PizzaHelperRecordService pizzaHelperRecordService;

    public RecordService(IServiceProvider serviceProvider)
    {
        memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
        appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
        rankService = serviceProvider.GetRequiredService<IRankService>();
        expireService = serviceProvider.GetRequiredService<ExpireService>();
        pizzaHelperRecordService = serviceProvider.GetRequiredService<PizzaHelperRecordService>();
    }

    public async ValueTask<RecordUploadResult> ProcessUploadAsync(SimpleRecord record)
    {
        if (IsStatisticsServiceWorking())
        {
            return RecordUploadResult.ComputingStatistics;
        }

        if (appDbContext.BannedList.Any(banned => banned.Uid == record.Uid))
        {
            return RecordUploadResult.UidBanned;
        }

        if (record.SpiralAbyss.ScheduleId != SpiralAbyssScheduleId.GetForNow())
        {
            return RecordUploadResult.NotCurrentSchedule;
        }

        if (!record.Validate())
        {
            return RecordUploadResult.InvalidData;
        }

        if (UploadingUids.TryGetValue(record.Uid, out _))
        {
            return RecordUploadResult.ConcurrencyNotSupported;
        }

        if (!UploadingUids.TryAdd(record.Uid, default))
        {
            return RecordUploadResult.ConcurrencyStateErrorAdd;
        }

        RecordUploadResult result = await SaveRecordAsync(record).ConfigureAwait(false);
        await pizzaHelperRecordService.TryPostRecordAsync(record).ConfigureAwait(false);

        if (!UploadingUids.TryRemove(record.Uid, out _))
        {
            return RecordUploadResult.ConcurrencyStateErrorRemove;
        }

        return result;
    }

    public ValueTask<bool> HaveUidUploadedAsync(string uid)
    {
        return HaveUploadedForCurrentScheduleAsync(uid);
    }

    public bool IsStatisticsServiceWorking()
    {
        return memoryCache.TryGetValue(StatisticsService.Working, out _);
    }

    private static EntityRecord SimpleRecordToEntity(SimpleRecord simpleRecord)
    {
        return new()
        {
            Uid = simpleRecord.Uid,
            Uploader = simpleRecord.Identity,
            UploadTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
        };
    }

    private static EntityAvatar SimpleAvatarToEntity(SimpleAvatar simpleAvatar, long recordId)
    {
        Dictionary<int, int> relicSetCounter = [];
        foreach (int id in simpleAvatar.ReliquarySetIds)
        {
            relicSetCounter.IncreaseOne(id);
        }

        foreach (int id in relicSetCounter.Keys)
        {
            if (relicSetCounter[id] >= 4)
            {
                relicSetCounter[id] = 4;
            }
            else if (relicSetCounter[id] >= 2)
            {
                relicSetCounter[id] = 2;
            }
            else
            {
                relicSetCounter.Remove(id);
            }
        }

        return new()
        {
            RecordId = recordId,
            AvatarId = simpleAvatar.AvatarId,
            WeaponId = simpleAvatar.WeaponId,
            ReliquarySet = string.Join(',', relicSetCounter.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key}-{kvp.Value}")),
            ActivedConstellationNumber = simpleAvatar.ActivedConstellationNumber,
        };
    }

    private static EntitySpiralAbyss SimpleSpiralAbyssToEntity(SimpleSpiralAbyss simpleSpiralAbyss, long recordId)
    {
        return new()
        {
            RecordId = recordId,
            TotalBattleTimes = simpleSpiralAbyss.TotalBattleTimes,
            TotalWinTimes = simpleSpiralAbyss.TotalWinTimes,
        };
    }

    private static EntityFloor SimpleFloorToEntity(SimpleFloor simpleFloor, long spiralAbyssId)
    {
        // Sort team avatars.
        foreach (ref readonly SimpleLevel level in CollectionsMarshal.AsSpan(simpleFloor.Levels))
        {
            foreach (ref readonly SimpleBattle battle in CollectionsMarshal.AsSpan(level.Battles))
            {
                battle.Avatars.Sort();
            }
        }

        return new()
        {
            SpiralAbyssId = spiralAbyssId,
            Index = simpleFloor.Index,
            Star = simpleFloor.Star,
            Levels = simpleFloor.Levels,
        };
    }

    private static EntityDamageRank SimpleRankToEntityDamageRank(SimpleRank rank, long spiralAbyssId, string uid)
    {
        return new()
        {
            SpiralAbyssId = spiralAbyssId,
            AvatarId = rank.AvatarId,
            Value = rank.Value,
            Uid = uid,
        };
    }

    private static EntityTakeDamageRank SimpleRankToEntityTakeDamageRank(SimpleRank rank, long spiralAbyssId, string uid)
    {
        return new()
        {
            SpiralAbyssId = spiralAbyssId,
            AvatarId = rank.AvatarId,
            Value = rank.Value,
            Uid = uid,
        };
    }

    private async Task<RecordUploadResult> SaveRecordAsync(SimpleRecord record)
    {
        bool haveUploaded = await HaveUploadedForCurrentScheduleAsync(record.Uid).ConfigureAwait(false);
        RecordUploadResult result = await GetNonErrorRecordUploadResult(record, haveUploaded).ConfigureAwait(false);

        using (IDbContextTransaction transaction = await appDbContext.Database.BeginTransactionAsync())
        {
            if (haveUploaded)
            {
                await appDbContext.Records.AsNoTracking().Where(r => r.Uid == record.Uid).ExecuteDeleteAsync().ConfigureAwait(false);
            }

            // EntityRecord
            EntityRecord entityRecord = SimpleRecordToEntity(record);
            await appDbContext.Records.AddAndSaveAsync(entityRecord).ConfigureAwait(false);
            long recordId = entityRecord.PrimaryId;

            // EntityAvatars
            List<EntityAvatar> entityAvatars = record.Avatars.SelectList(a => SimpleAvatarToEntity(a, recordId));
            await appDbContext.Avatars.AddRangeAndSaveAsync(entityAvatars).ConfigureAwait(false);

            // EntitySpiralAbyss
            EntitySpiralAbyss entitySpiralAbyss = SimpleSpiralAbyssToEntity(record.SpiralAbyss, entityRecord.PrimaryId);
            await appDbContext.SpiralAbysses.AddAndSaveAsync(entitySpiralAbyss).ConfigureAwait(false);
            long spiralAbyssId = entitySpiralAbyss.PrimaryId;

            // EntityFloors
            List<EntityFloor> entityFloors = record.SpiralAbyss.Floors.Where(f => f.Index >= 9).Select(f => SimpleFloorToEntity(f, spiralAbyssId)).ToList();
            await appDbContext.SpiralAbyssFloors.AddRangeAndSaveAsync(entityFloors).ConfigureAwait(false);

            // EntityDamageRank
            // People must deal damage anyway.
            EntityDamageRank entityDamageRank = SimpleRankToEntityDamageRank(record.SpiralAbyss.Damage, spiralAbyssId, record.Uid);
            await appDbContext.DamageRanks.AddAndSaveAsync(entityDamageRank).ConfigureAwait(false);

            // EntityTakeDamageRank
            if (record.SpiralAbyss.TakeDamage != null)
            {
                EntityTakeDamageRank entityTakeDamageRank = SimpleRankToEntityTakeDamageRank(record.SpiralAbyss.TakeDamage, spiralAbyssId, record.Uid);
                await appDbContext.TakeDamageRanks.AddAndSaveAsync(entityTakeDamageRank).ConfigureAwait(false);
            }

            transaction.Commit();
        }

        // Redis rank sync
        await rankService.SaveRankAsync(record.Uid, record.SpiralAbyss.Damage, record.SpiralAbyss.TakeDamage).ConfigureAwait(false);

        return result;
    }

    private async ValueTask<bool> HaveUploadedForCurrentScheduleAsync(string uid)
    {
        EntityRecord? entityRecord = await appDbContext.Records.SingleOrDefaultAsync(r => r.Uid == uid).ConfigureAwait(false);

        if (entityRecord == null)
        {
            return false;
        }

        if (!await appDbContext.SpiralAbysses.AnyAsync(s => s.RecordId == entityRecord.PrimaryId).ConfigureAwait(false))
        {
            return false;
        }

        return true;
    }

    private async ValueTask<RecordUploadResult> GetNonErrorRecordUploadResult(SimpleRecord record, bool haveUploadedForCurrentSchedule)
    {
        if (haveUploadedForCurrentSchedule)
        {
            return RecordUploadResult.OkWithNotFirstAttempt;
        }

        if (record.Identity is not UploaderIdentities.SnapHutao)
        {
            return RecordUploadResult.OkWithNotSnapHutaoClient;
        }

        if (record.ReservedUserName == null)
        {
            return RecordUploadResult.OkWithNoUserNamePresented;
        }

        GachaLogTermExtendResult result = await expireService.ExtendGachaLogTermForUserNameAsync(record.ReservedUserName, GachaLogExtendDays).ConfigureAwait(false);
        if (result.Kind is not GachaLogTermExtendResultKind.Ok)
        {
            return RecordUploadResult.OkWithGachaLogNoSuchUser;
        }

        return RecordUploadResult.OkWithGachaLogExtented;
    }

    private struct UploadToken;
}