using System.ComponentModel.DataAnnotations.Schema;//这是一个EntityFrameworkCore的命名空间 是为了使用ForeignKey
using AuctionService.Entities;//引入Entities命名空间，以便使用Auction类

namespace AuctionService;

[Table("Items")]//这个是为了在数据库中创建一个名为Items的表
//没有这个特性，EF Core 会默认使用类名（在本例中是 Item）来命名数据库表。
//如果使用 [Table("Items")]，则确保表名为复数形式
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