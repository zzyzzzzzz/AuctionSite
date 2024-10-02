namespace AuctionService;

public class Item
{
    public Guid Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }
    public string ImageUrl { get; set; }

    // nav property 导航属性用于表示实体之间的关系。
    // 在这个上下文中，Auction 和 Item 之间存在一种关联关系
    public Auction Auction { get; set; }//是导航属性，用于表示与该Item相关的拍卖对象。
    public Guid AuctionId { get; set; } //是外键，指向对应的Auction的ID,用于数据库关联。


}