using Microsoft.EntityFrameworkCore;
using AuctionService.Entities;
using MassTransit;
// 引入EntityFrameworkCore的命名空间，方便使用DbContext
//EF Core 是一个流行的 ORM（对象关系映射）框架，允许开发者使用 .NET 对象来与数据库交互。
//引入这个命名空间后，你可以直接使用其中的类和功能，比如 DbContext 和 DbSet。

namespace AuctionService.Data//// 定义一个名为AuctionService.Data的命名空间
{
    // 继承DbContext以使用EntityFrameworkCore的功能
    //DbContext 是 EF Core 的核心类之一，负责管理实体与数据库之间的关系。
    public class AuctionDbContext : DbContext 

    {
        //这是 AuctionDbContext 的构造函数。构造函数的目的是初始化类的实例。
        //这个构造函数接收一个 DbContextOptions 类型的参数，并将其传递给基类的构造函数（即 DbContext）。
        //DbContextOptions 包含了数据库连接的设置，如数据库类型和连接字符串等
        public AuctionDbContext(DbContextOptions options) : base(options) 
        {
        }

       //Basic CRUD operations
        public DbSet<Auction> Auctions { get; set; }  // 创建一个名为Auctions的表//then give a pluralized name使用复数名 for the item 当定义 DbSet 时，应该使用复数形式的名称，以便更清楚地表达该集合是多个实体的集合
    
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }


}