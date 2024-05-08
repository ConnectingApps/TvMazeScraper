using TvMazeScraper.Api.InternalModels;

namespace TvMazeScraper.Api;

public interface IDataAccess
{
    Task<IEnumerable<ShowResponse>> GetResponsesAsync(int pageNumber, int pageSize);
}