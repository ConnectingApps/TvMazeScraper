using TvMazeScraper.Api.InternalModels;

namespace TvScraper.Api.IntegrationTest;
using Refit;

public interface IShowApi
{
    [Get("/Show")]
    Task<ApiResponse<List<ShowResponse>>>
        GetShowDataAsync([AliasAs("pageNumber")] int pageNumber, [AliasAs("pageSize")] int pageSize);

}