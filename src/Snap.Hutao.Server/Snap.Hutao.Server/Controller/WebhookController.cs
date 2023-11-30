﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.Model.Afdian;
using Snap.Hutao.Server.Option;
using Snap.Hutao.Server.Service;
using Snap.Hutao.Server.Service.Afdian;
using Snap.Hutao.Server.Service.GachaLog;

namespace Snap.Hutao.Server.Controller;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class WebhookController : ControllerBase
{
    private readonly AfdianWebhookService afdianWebhookService;
    private readonly ExpireService expireService;
    private readonly MailService mailService;
    private readonly HttpClient httpClient;
    private readonly IMemoryCache memoryCache;
    private readonly ILogger<WebhookController> logger;
    private readonly AfdianOptions afdianOptions;

    public WebhookController(IServiceProvider serviceProvider)
    {
        expireService = serviceProvider.GetRequiredService<ExpireService>();
        mailService = serviceProvider.GetRequiredService<MailService>();
        httpClient = serviceProvider.GetRequiredService<HttpClient>();
        memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
        logger = serviceProvider.GetRequiredService<ILogger<WebhookController>>();
    }

    [HttpGet("Incoming/Afdian")]
    [HttpPost("Incoming/Afdian")]
    public async Task<IActionResult> IncomingAfdianAsync([FromBody] AfdianResponse<OrderWrapper> request)
    {
        string tradeNumber = request.Data.Order.OutTradeNo;
        string key = $"Afdian_Trade:{tradeNumber}";
        if (!memoryCache.TryGetValue(key, out _))
        {
            // prevent multiple activate
            memoryCache.Set(key, new HookToken(), TimeSpan.FromHours(1));

            string userName = request.Data.Order.Remark;
            logger.LogInformation("UserName:[{name}]", request.Data.Order.Remark);

            if (request.Data.Order.SkuDetail.FirstOrDefault() is SkuDetail skuDetail)
            {
                // GachaLog Upload
                if (skuDetail.SkuId == afdianOptions.SkuGachaLogUploadService)
                {
                    string skuId = skuDetail.SkuId;
                    int count = skuDetail.Count;

                    if (await ValidateTradeAsync(tradeNumber, skuId, count).ConfigureAwait(false))
                    {
                        GachaLogTermExtendResult result = await expireService.ExtendGachaLogTermForUserNameAsync(userName, 30 * count).ConfigureAwait(false);

                        if (result.Kind is GachaLogTermExtendResultKind.Ok)
                        {
                            await mailService.SendPurchaseGachaLogStorageServiceAsync(userName, result.ExpiredAt.ToString("yyy/MM/dd HH:mm:ss"), tradeNumber).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        logger.LogInformation("Validation failed");
                    }
                }
                else
                {
                    logger.LogInformation("SKU Id:[{id}] not supported", skuDetail.SkuId);
                }
            }
            else
            {
                logger.LogInformation("No SKU info");
            }
        }

        return new JsonResult(new AfdianResponse() { ErrorCode = 200, ErrorMessage = string.Empty });
    }

    private async Task<bool> ValidateTradeAsync(string tradeNumber, string skuId, int count)
    {
        QueryOrder query = QueryOrder.Create(afdianOptions.UserId, tradeNumber, afdianOptions.UserToken);
        logger.LogInformation("Fetch data for trade: [{trade}]", tradeNumber);
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("https://afdian.net/api/open/query-order", query).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        AfdianResponse<ListWrapper<Order>>? resp = await response.Content.ReadFromJsonAsync<AfdianResponse<ListWrapper<Order>>>().ConfigureAwait(false);

        if (resp != null && resp.ErrorCode == 200)
        {
            if (resp.Data.List.FirstOrDefault() is Order order)
            {
                if (order.SkuDetail.FirstOrDefault() is SkuDetail skuDetail)
                {
                    if (order.OutTradeNo == tradeNumber && skuDetail.SkuId == skuId && skuDetail.Count == count)
                    {
                        return true;
                    }
                    else
                    {
                        logger.LogInformation("Detail not matched");
                    }
                }
                else
                {
                    logger.LogInformation("No sku");
                }
            }
            else
            {
                logger.LogInformation("No matched order");
            }
        }
        else
        {
            logger.LogInformation("Bad Request, Not valid data");
        }

        return false;
    }

    /// <summary>
    /// 并行 Hook 占位
    /// </summary>
    private struct HookToken
    {
    }
}