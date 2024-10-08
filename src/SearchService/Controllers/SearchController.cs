using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService;

 
[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Item, Item>(); //不用Find了 用PagedSearch
        
        //因为下面有新的sorting了所以不需要这个了
        //query.Sort(x => x.Ascending(a => a.Make));

        if(!string.IsNullOrEmpty(searchParams.SearchTerm))
        {//if we do have the string  // 如果有搜索条件，打印出来//Console.WriteLine($"Search term: {searchTerm}");
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        //这里是filter 
        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };

        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
            && x.AuctionEnd > DateTime.UtcNow),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };

        if(!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        if(!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        // 打印结果以帮助调试
        //Console.WriteLine($"Results Count: {result.Results.Count}, Total Count: {result.TotalCount}");


        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }

}