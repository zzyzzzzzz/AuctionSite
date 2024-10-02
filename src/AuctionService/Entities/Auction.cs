namespace AuctionService.Entities;
    // 1.命名空间用于组织代码和避免名称冲突。
    // 它并不一定与文件夹结构相对应。 
    // 例如，AuctionService.Entities 可以在多个不同的文件夹中定义。

public class Auction
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string Seller { get; set; }
    public string Winner { get; set; }
    public int? SoldAmount { get; set; }
    public int? CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //因为postgres默认utc时间
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
    public DateTime AuctionEnd { get; set; }
    public Status Status { get; set; }
    public Item Item { get; set; }    
}