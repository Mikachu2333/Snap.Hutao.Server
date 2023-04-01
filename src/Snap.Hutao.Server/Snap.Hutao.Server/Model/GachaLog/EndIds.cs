﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.Model.Upload;
using System.Collections.Immutable;

namespace Snap.Hutao.Server.Model.GachaLog;

/// <summary>
/// Id集合
/// </summary>
public sealed class EndIds : Dictionary<string, long>
{
    /// <summary>
    /// 查询类型
    /// </summary>
    public static readonly ImmutableList<GachaConfigType> QueryTypes = new List<GachaConfigType>
    {
        GachaConfigType.NoviceWish,
        GachaConfigType.StandardWish,
        GachaConfigType.AvatarEventWish,
        GachaConfigType.WeaponEventWish,
    }.ToImmutableList();

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="id">id</param>
    public void Add(GachaConfigType type, long id)
    {
        Add($"{(int)type}", id);
    }
}