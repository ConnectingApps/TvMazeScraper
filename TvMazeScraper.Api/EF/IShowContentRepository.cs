using TvMaze.Client;

namespace TvMazeScraper.Api.EF;

public interface IShowContentRepository
{
    Task<bool> RecordExistsAsync(int externalId);
    Task<ShowContent?[]> GetMultiAsync(int[]? externalIds);
    Task<ShowContent?> GetAsync(int externalId);
    Task CreateRecordAsync(int externalId, Show content);
    Task CreateRecordsAsync((int externalId, Show content)[] records);
}