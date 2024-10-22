using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly AuctionDbContext _dbContext;

    //injected dbcontext
    public AuctionFinishedConsumer(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //这会是一个async方法，因为我们需要等待数据库操作完成
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("--> Consuming auction finished");

        var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);//给我们信息用于找到对应的拍卖
        //检查受否sold
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount;
        }
        
        auction.Status = auction.SoldAmount > auction.ReservePrice
            ?Status.Finished:Status.ReserveNotMet;
        
        await _dbContext.SaveChangesAsync();
    }
}
