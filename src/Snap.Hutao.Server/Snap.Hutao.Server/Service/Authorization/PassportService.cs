﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Snap.Hutao.Server.Model.Entity;
using Snap.Hutao.Server.Model.Passport;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Snap.Hutao.Server.Service.Authorization;

/// <summary>
/// 通行证服务
/// </summary>
public class PassportService
{
    private readonly UserManager<HutaoUser> userManager;
    private readonly string rsaPrivateKey;
    private readonly SymmetricSecurityKey jwtSigningKey;

    /// <summary>
    /// 构造一个新的通行证服务
    /// </summary>
    /// <param name="userManager">用户管理器</param>
    /// <param name="configuration">配置</param>
    public PassportService(UserManager<HutaoUser> userManager, IConfiguration configuration)
    {
        this.userManager = userManager;

        rsaPrivateKey = configuration["RSAPrivateKey"]!;
        jwtSigningKey = new(Encoding.UTF8.GetBytes(configuration["Jwt"]!));
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="source">密文</param>
    /// <returns>解密的文本</returns>
    public string Decrypt(string source)
    {
        byte[] encryptedBytes = Convert.FromBase64String(source);
        using (RSACryptoServiceProvider rsa = new(2048))
        {
            rsa.ImportFromPem(rsaPrivateKey);
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, true);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

    /// <summary>
    /// 异步注册
    /// </summary>
    /// <param name="passport">账密</param>
    /// <returns>结果</returns>
    public async Task<PassportResult> RegisterAsync(Passport passport)
    {
        if (await userManager.FindByNameAsync(passport.UserName).ConfigureAwait(false) is HutaoUser user)
        {
            return new(false, "邮箱已被注册");
        }
        else
        {
            HutaoUser newUser = new() { UserName = passport.UserName };
            IdentityResult result = await userManager.CreateAsync(newUser, passport.Password).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return new(true, "注册成功", CreateToken(newUser));
            }
            else
            {
                return new(false, "数据库错误");
            }
        }
    }

    /// <summary>
    /// 异步修改密码
    /// </summary>
    /// <param name="passport">账密</param>
    /// <returns>结果</returns>
    public async Task<PassportResult> ResetPasswordAsync(Passport passport)
    {
        if (await userManager.FindByNameAsync(passport.UserName).ConfigureAwait(false) is HutaoUser user)
        {
            await userManager.RemovePasswordAsync(user).ConfigureAwait(false);
            await userManager.AddPasswordAsync(user, passport.Password).ConfigureAwait(false);

            return new(true, "密码设置成功");
        }
        else
        {
            return new(false, "该邮箱尚未注册");
        }
    }

    /// <summary>
    /// 异步登录
    /// </summary>
    /// <param name="passport">账密</param>
    /// <returns>结果</returns>
    public async Task<PassportResult> LoginAsync(Passport passport)
    {
        if (await userManager.FindByNameAsync(passport.UserName).ConfigureAwait(false) is HutaoUser user)
        {
            if (await userManager.CheckPasswordAsync(user, passport.Password).ConfigureAwait(false))
            {
                return new(true, "登录成功", CreateToken(user));
            }
        }

        return new PassportResult(false, "邮箱或密码不正确");
    }

    private string CreateToken(HutaoUser user)
    {
        JwtSecurityTokenHandler handler = new();
        SecurityTokenDescriptor descriptor = new()
        {
            Subject = new(new Claim[]
            {
                new Claim(PassportClaimTypes.UserId, user.Id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = "homa.snapgenshin.com",
            SigningCredentials = new(jwtSigningKey, SecurityAlgorithms.HmacSha256Signature),
        };

        SecurityToken token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
}