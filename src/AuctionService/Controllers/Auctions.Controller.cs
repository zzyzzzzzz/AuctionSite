using AutoMapper;
using AuctionService.Data;
using Microsoft.AspNetCore.Mvc;
using AuctionService.DTOs;
using Microsoft.EntityFrameworkCore;
using AuctionService.Entities;
using AutoMapper.QueryableExtensions;


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
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date) //这里我们需要一个参数来过滤拍卖品
    {

        var query = _context.Auctions.OrderBy(x=>x.Item.Make).AsQueryable();

        if(!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0) ;//退回超过这个日期的拍卖品

        }

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync(); //这里我们使用ProjectTo方法来映射我们的数据  

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

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)

    {
        var auction = _mapper.Map<Auction>(auctionDto);
         
        //TODO: Add current user as seller
        auction.Seller = "test";

        _context.Auctions.Add(auction);

        var result = await _context.SaveChangesAsync()> 0 ; //if is 0 then means no changes were made

        if(!result) return BadRequest("Could not save changes to the database");

        //我们还需要告诉客户端我们已经成功创建了一个拍卖品
        //我们想要告诉them the location of the newly created resource which is GetAuctionById
        return CreatedAtAction(nameof(GetAuctionById), new {auction.Id}, _mapper.Map<AuctionDto>(auction));//从entity映射到dto
    }


    [HttpPut("{id}")]
    //我们可以在这里返回一个没有任何类型参数的操作结果，并指定其名称,因为我们不需要返回任何数据
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        //我们需要首先从与此特定ID匹配的数据库中获取auction,然后我们可以更新它
        var auction = await _context.Auctions.Include(x=>x.Item)
            .FirstOrDefaultAsync(x=>x.Id == id);

            //然后检查是否找到了这个auction
            if(auction == null) return NotFound();   //没在database找到能update的
            //TODO： Check if seller == username
            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage; 
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
            //这后面我们需要把UpdateAuctionDto中的int 转为 int? 因为int?可以为null

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok();
            return BadRequest("Problem saving changes");
    }





    [HttpDelete("{id}")] //删除一个拍卖品 这里要id是因为我们需要知道删除哪个拍卖品
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions.FindAsync(id);
        if(auction == null) return NotFound();

        //TODO: Check if seller == username

        _context.Auctions.Remove(auction);
        var result = await _context.SaveChangesAsync() > 0;
        if(!result) return BadRequest("Problem deleting the auction");
        return Ok();
    }
}