using TvMaze.Client;
using TvMazeScraper.Api.EF;
using TvMazeScraper.Api.InternalModels;
using Cast = TvMazeScraper.Api.InternalModels.Cast;

namespace TvMazeScraper.Api;

public class DataAccess : IDataAccess
{
    private readonly IMazeApi _refitClient;
    private readonly IShowContentRepository _showContentRepository;
    private readonly ILogger _logger;

    public DataAccess(IMazeApi refitClient, 
        IShowContentRepository showContentRepository,
        ILogger<DataAccess> logger)
    {
        _refitClient = refitClient;
        _showContentRepository = showContentRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ShowResponse>> GetResponsesAsync(int pageNumber, int pageSize)
    {
        var requestsTasks = Enumerable.Range(pageNumber, pageSize)
            .Select(GetTvMazeAsync)
            .ToList();
        var tvMazeResponses = await Task.WhenAll(requestsTasks);
        return tvMazeResponses.Select(ToShowResponse);
    }

    private async Task<Show> GetTvMazeAsync(int id)
    {
        var fromDb = await _showContentRepository.GetAsync(id);
        if (fromDb == null)
        {
            var fromApi = await CallTvMazeAsync(id);
            await _showContentRepository.CreateRecordAsync(id, fromApi);
            return fromApi;
        }
        return fromDb.Content;
    }

    private static ShowResponse ToShowResponse(Show show)
    {
        return new ShowResponse
        {
            Id = show.Id,
            Name = show.Name,
            Cast = show.Embedded.Cast.Select(c => new Cast
            {
                Id = c.Character.Id,
                Name = c.Person.Name,
                Birthday = c.Person.Birthday
            }).OrderByDescending(d => d.Birthday == null ? DateOnly.MinValue : DateOnly.Parse(d.Birthday)).ToArray()
        };
    }

    private async Task<Show> CallTvMazeAsync(int id)
    {
        string? foundError = null;
        try
        {
            var response = await _refitClient.GetShowWithDetailsAsync(id, "cast");
            foundError = response.Error?.Content;
            await response.EnsureSuccessStatusCodeAsync();
            return response.Content!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Calling TV Maze failed with found error {foundError}");
            throw;
        }
    }
}