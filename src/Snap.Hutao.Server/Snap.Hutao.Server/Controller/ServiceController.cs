﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.Controller.Filter;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity;
using Snap.Hutao.Server.Model.Response;
using Snap.Hutao.Server.Model.Upload;

namespace Snap.Hutao.Server.Controller;

[Authorize]
[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(RequestFilter))]
[ApiExplorerSettings(IgnoreApi = true)]
public class ServiceController : ControllerBase
{
    private const string DefaultLocale = "CHS";
    private static readonly List<string> Locales = new()
    {
        DefaultLocale, "CHT", "DE", "EN", "ES",
        "FR", "ID", "IT", "JP", "KR",
        "PT", "RU", "TH", "TR", "VI",
    };

    private readonly AppDbContext appDbContext;
    private readonly UserManager<HutaoUser> userManager;

    public ServiceController(AppDbContext appDbContext, UserManager<HutaoUser> userManager)
    {
        this.appDbContext = appDbContext;
        this.userManager = userManager;
    }

    [HttpGet("GachaLog/Compensation")]
    public async Task<IActionResult> GachaLogCompensationAsync([FromQuery] int days)
    {
        if (!await IsMaintainerAsync(this.GetUserId()).ConfigureAwait(false))
        {
            return Model.Response.Response.Fail(ReturnCode.ServiceKeyInvalid, "只有官方人员可以这么做");
        }

        long nowTick = DateTimeOffset.Now.ToUnixTimeSeconds();
        await appDbContext.Users
            .Where(user => user.GachaLogExpireAt < nowTick)
            .ExecuteUpdateAsync(user => user.SetProperty(u => u.GachaLogExpireAt, u => nowTick))
            .ConfigureAwait(false);

        int seconds = days * 24 * 60 * 60;

        await appDbContext.Users
            .ExecuteUpdateAsync(user => user.SetProperty(u => u.GachaLogExpireAt, u => u.GachaLogExpireAt + seconds))
            .ConfigureAwait(false);

        nowTick += seconds;
        return Model.Response.Response.Success($"操作成功，增加了 {days} 天时长，到期时间: {DateTimeOffset.FromUnixTimeSeconds(nowTick)}");
    }

    [HttpGet("GachaLog/Designation")]
    public async Task<IActionResult> GachaLogDesignationAsync([FromQuery] string userName, [FromQuery] int days)
    {
        if (!await IsMaintainerAsync(this.GetUserId()).ConfigureAwait(false))
        {
            return Model.Response.Response.Fail(ReturnCode.ServiceKeyInvalid, "只有官方人员可以这么做");
        }

        if (await userManager.FindByNameAsync(userName).ConfigureAwait(false) is { } user)
        {
            long nowTick = DateTimeOffset.Now.ToUnixTimeSeconds();
            if (user.GachaLogExpireAt < nowTick)
            {
                user.GachaLogExpireAt = nowTick;
            }

            int seconds = days * 24 * 60 * 60;
            user.GachaLogExpireAt += seconds;
            await userManager.UpdateAsync(user).ConfigureAwait(false);
            nowTick += seconds;
            return Model.Response.Response.Success($"操作成功，增加了 {days} 天时长，到期时间: {DateTimeOffset.FromUnixTimeSeconds(nowTick)}");
        }

        return Model.Response.Response.Fail(ReturnCode.UserNameNotExists, $"用户名不存在");
    }

    [HttpPost("Announcement/Upload")]
    public async Task<IActionResult> UploadAnnouncementAsync(HutaoUploadAnnouncement announcement)
    {
        foreach (string locale in Locales)
        {
            EntityAnnouncement entity = new()
            {
                Locale = locale,
                LastUpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Title = announcement.Title,
                Content = announcement.Content,
                Severity = announcement.Severity,
                Link = announcement.Link,
            };

            appDbContext.Announcements.Add(entity);
        }

        await appDbContext.SaveChangesAsync().ConfigureAwait(false);

        return Model.Response.Response.Success("上传公告成功");
    }

    private async Task<bool> IsMaintainerAsync(int userId)
    {
        HutaoUser? user = await appDbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Id == userId)
            .ConfigureAwait(false);

        if (user != null)
        {
            return user.IsMaintainer;
        }
        else
        {
            return false;
        }
    }
}