using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Api.InternalModels;

namespace TvMazeScraper.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ShowController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShowResponse>>> GetShows(int pageNumber = 1, int pageSize = 10)
    {
        throw new NotImplementedException();
    }
}