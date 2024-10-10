using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;

    //inject automapper
    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("--> Consuming auction created : "+context.Message.Id );
    
        var item = _mapper.Map<Item>(context.Message);

        //举一个特例，如果model是Foo，我们就抛出一个异常
        if(item.Model == "Foo") throw new ArgumentException("Cannot sell cars with name of Foo");

        await item.SaveAsync();//如果database save失败，会抛出异常
    }
}
