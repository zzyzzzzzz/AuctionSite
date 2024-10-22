using System;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("--> Consuming bid placed");

        //获取拍卖
        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

        //我们可能希望在搜索结果中显示特定拍卖的高价，这就是我们在搜索服务中这样做的原因

        if(context.Message.BidStatus.Contains("Accepted")
            && context.Message.Amount >auction.CurrentHighBid)
        {
            auction.CurrentHighBid = context.Message.Amount;
            await auction.SaveAsync();
        }
    }
}
