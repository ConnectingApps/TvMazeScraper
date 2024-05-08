using TvMaze.Client;

namespace TvMazeScraper.Api.EF;

public interface IShowContentRepository
{
    Task<ShowContent?> GetAsync(int externalId);
    Task<bool> RecordExistsAsync(int externalId);
    Task CreateRecordAsync(int externalId, Show content);
}