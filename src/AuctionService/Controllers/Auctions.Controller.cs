using AutoMapper;
using AuctionService.Data;
using Microsoft.AspNetCore.Mvc;
using AuctionService.DTOs;
using Microsoft.EntityFrameworkCore;


namespace AuctionService.Controllers;


[ApiController]
[Route("api/auctions")]



//用于构建不带视图View的 MVC 控制器。
//(为什么要不带视图的)因为我们的API不需要视图，只需要返回数据
public class AuctionsController : ControllerBase //ControllerBase是一个ASP.NET Core MVC控制器的基类，它提供了一些方法和属性，用于处理HTTP请求。
{
    private readonly AuctionDbContext _context; //数据库上下文 用于连接数据库 
    private readonly IMapper _mapper; //AutoMapper 用于映射数据
    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet] //为了获取所<有拍卖品
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var auctions = await _context.Auctions 
            //relatied property
            .Include(x=>x.Item)
            .OrderBy(x=>x.Item.Make)
            .ToListAsync();

        return _mapper.Map<List<AuctionDto>>(auctions);

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
         var auction = await _context.Auctions
            .Include(x=>x.Item)
            .FirstOrDefaultAsync(x=>x.Id == id);

        if(auction == null) return NotFound();
        return _mapper.Map<AuctionDto>(auction);
        
    }
 
}