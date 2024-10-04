using AuctionService.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// 注册数据库上下文（AuctionDbContext）并配置Npgsql（PostgreSQL的EF Core提供程序）
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));// 从配置文件中获取数据库连接字符串，并使用Npgsql作为数据库提供程序
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());// 注册AutoMapper服务,后面的是location of the assemblies

var app = builder.Build();// 构建应用程序



// Configure the HTTP request pipeline.

app.UseAuthorization();// 启用授权中间件

app.MapControllers();// 映射控制器到应用程序的请求


try
{
    DbInitializer.InitDb(app);// 初始化数据库
}
catch (Exception e)  //exception handling
{
    Console.WriteLine(e);
}

app.Run();// 运行应用程序
