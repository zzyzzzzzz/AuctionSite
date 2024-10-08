using System.Net;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Models;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//加上addpolicyhandler方法 
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

//新加上lifetime 
app.Lifetime.ApplicationStarted.Register(async () =>
{
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
});




app.Run();

//if auction service is down, we want to retry the request after 3 seconds until it is back up

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
    .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
     