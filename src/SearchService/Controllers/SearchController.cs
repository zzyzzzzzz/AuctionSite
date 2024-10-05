using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

 
[ApiController]
[Route("api/search")]
public class SearchService : ControllerBase
{
    [HttpGet]
    
    public async Task<ActionResult<List<Item>>> SearchItems(string searchTerm, 
        int pageNumber = 1, int pageSize = 4)
    {
        var query = DB.PagedSearch<Item>(); //不用Find了 用PagedSearch
        
        query.Sort(x => x.Ascending(a => a.Make));

        if(!string.IsNullOrEmpty(searchTerm))
        {//if we do have the string
        // 如果有搜索条件，打印出来
            //Console.WriteLine($"Search term: {searchTerm}");

            query.Match(Search.Full, searchTerm).SortByTextScore();
        }

        query.PageNumber(pageNumber);
        query.PageSize(pageSize);

        var result = await query.ExecuteAsync();

        // 打印结果以帮助调试
        //Console.WriteLine($"Results Count: {result.Results.Count}, Total Count: {result.TotalCount}");


        return Ok(new{
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }

}