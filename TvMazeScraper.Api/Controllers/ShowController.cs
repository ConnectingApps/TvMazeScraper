using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Api.InternalModels;

namespace TvMazeScraper.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ShowController : ControllerBase
{
    private readonly IDataAccess _dataAccess;

    public ShowController(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShowResponse>>> GetShows(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Wrong parameter values");
        }
        var toReturn = await _dataAccess.GetResponsesAsync(pageNumber, pageSize);
        return Ok(toReturn.ToList());
    }
}