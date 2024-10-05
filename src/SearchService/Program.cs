using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Data;
using SearchService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();


//initialize our mongo db database by calling the InitDb method in DbInitializer.cs
//use try catch block to catch any exceptions that might be thrown
//使用await关键字调用InitDb方法，因为它是一个异步方法
try
{
    await DbInitializer.InitDb(app);
}catch(Exception e)
{
    Console.WriteLine(e);
}

app.Run();
