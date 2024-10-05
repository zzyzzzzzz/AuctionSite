using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

 
[ApiController]
[Route("api/search")]
public class SearchService : ControllerBase
{
    [HttpGet]
    
    public async Task<ActionResult<List<Item>>> SearchItems(string searchTerm)
    {
        //然后去看mongodb的文档
        var query = DB.Find<Item>();
        query.Sort(x => x.Ascending(a => a.Make));

        if(!string.IsNullOrEmpty(searchTerm))
        {//if we do have the string
            query.Match(Search.Full, searchTerm).SortByTextScore();
        }

        var result = await query.ExecuteAsync();
        return result;
    }

}