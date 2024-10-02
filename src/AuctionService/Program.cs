using AuctionService;
using AuctionService.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();// 注册控制器服务，以处理HTTP请求。

//// 注册数据库上下文（AuctionDbContext）并配置Npgsql（PostgreSQL的EF Core提供程序）
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    // 从配置文件中获取数据库连接字符串，并使用Npgsql作为数据库提供程序
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
}

);
    

var app = builder.Build();// 构建应用程序

// Configure the HTTP request pipeline.


app.UseAuthorization();// 启用授权中间件

app.MapControllers();// 映射控制器到应用程序的请求


app.Run();// 运行应用程序
