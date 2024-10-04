using System;
using AutoMapper;
using AuctionService.Entities;
using AuctionService.DTOs;
namespace AuctionService.RequestHelpers;

//profiles是AutoMapper的一个核心类，用于创建映射关系
public class MappingProfiles:Profile 
{
//empty constructor
    public MappingProfiles()//这个构造函数用于创建映射关系 即将Auction映射到AuctionDto
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);  
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Auction>()
            .ForMember(d => d.Item, o =>o.MapFrom(s =>s));
        CreateMap<CreateAuctionDto, Item>();

        //CreateMap是AutoMapper的一个方法，用于创建映射关系
        //语法是CreateMap<源类型，目标类型>()
        //IncludeMembers是AutoMapper的一个方法，用于包含源类型的所有成员
        // x是一个lambda表达式，表示源类型的所有成员 x.Item表示源类型的Item成员

        //这个是将Item映射到AuctionDto  
        //这里没有使用IncludeMembers方法，因为Item类中没有导航属性

        //这个是将CreateAuctionDto映射到Auction
            //ForMember是AutoMapper的一个方法，用于指定源类型和目标类型的成员
            //d是目标类型的成员，o是源类型的成员, s是源类型
            //MapFrom是AutoMapper的一个方法，用于指定源类型的成员
            //这里的意思是将CreateAuctionDto的所有成员映射到Auction的Item成员

        //这个是将CreateAuctionDto映射到Item

        //最后还要把这个service放到program.cs中

    }
}

