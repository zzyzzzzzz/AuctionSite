using System;
using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly AuctionDbContext _dbContext;

    public BidPlacedConsumer(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        //当收到一个新的出价时
        //我们只会在没有物品相关实体的情况下创建一个新的拍卖
        //我们在拍卖中更新的东西只是在拍卖本身内部，而不是在物品内部,所以我们不需要物品实体
        Console.WriteLine("--> Consuming bid placed");//然后复制这个到AuctionFinishedConsumer.cs

        //注意这里要await,因为我们需要等待数据库操作完成
        var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);
    
        //这时候auction还没有Winner，也没有SoldAmount因为还没卖出，也不知道CurrentHighBid

        if(auction.CurrentHighBid == null 
        || context.Message.BidStatus.Contains("Accepted")
        && context.Message.Amount > auction.CurrentHighBid)
        {
            auction.CurrentHighBid = context.Message.Amount;
            await _dbContext.SaveChangesAsync();
        }
    }
}
