using System.Net;
using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService;
using SearchService.Data;
using SearchService.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//加上addpolicyhandler方法 
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit(X =>
{
    //需要告诉它在哪里可以找到我们正在创造的消费者
    //我们在同一名称空间中创建的任何其他消费者都将自动通过公共交通进行注册。
    X.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();


    //为了区分 endpoint 名称，我们将使用 kebab case，加上 search 前缀
    X.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

    
    X.UsingRabbitMq((context, cfg) =>
    {        
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
        {
            h.Username(builder.Configuration.GetValue("RabbitMQ:Username", "guest")!);
            h.Password(builder.Configuration.GetValue("RabbitMQ:Password", "guest")!);
        });
        
        cfg.ReceiveEndpoint("search-auction-created",e=>
        {
            e.UseMessageRetry(r => r.Interval(5,5));
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

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
     