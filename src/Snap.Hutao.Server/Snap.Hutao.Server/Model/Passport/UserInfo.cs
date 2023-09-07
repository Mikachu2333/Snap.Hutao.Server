﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Model.Passport;

/// <summary>
/// 用户信息
/// </summary>
public class UserInfo
{
    public string? NormalizedUserName { get; set; }

    /// <summary>
    /// 是否为开发者
    /// </summary>
    public bool IsLicensedDeveloper { get; set; }

    /// <summary>
    /// 是否为官方维护人员
    /// </summary>
    public bool IsMaintainer { get; set; }

    /// <summary>
    /// 祈愿记录服务到期时间
    /// </summary>
    public DateTimeOffset GachaLogExpireAt { get; set; }
}