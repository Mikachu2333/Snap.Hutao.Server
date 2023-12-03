﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Server.Option;

public sealed class AppOptions
{
    public string AfdianUserToken { get; set; } = default!;

    public string JwtRaw { get; set; } = default!;

    public string ReCaptchaKey { get; set; } = default!;

    public string RedisAddress { get; set; } = default!;

    public string RSAPrivateKey { get; set; } = default!;

    public AfdianOptions Afdian { get; set; } = default!;

    public DiscordOptions Discord { get; set; } = default!;

    public GenshinPizzaHelperOptions GenshinPizzaHelper { get; set; } = default!;

    public SmtpOptions Smtp { get; set; } = default!;
}