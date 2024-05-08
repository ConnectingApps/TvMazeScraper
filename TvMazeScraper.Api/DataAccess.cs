using TvMaze.Client;
using TvMazeScraper.Api.InternalModels;
using Cast = TvMazeScraper.Api.InternalModels.Cast;

namespace TvMazeScraper.Api;

public class DataAccess : IDataAccess
{
    private readonly IMazeApi _refitClient;
    private readonly ILogger _logger;

    public DataAccess(IMazeApi refitClient, ILogger<DataAccess> logger)
    {
        _refitClient = refitClient;
        _logger = logger;
    }

    public async Task<IEnumerable<ShowResponse>> GetResponsesAsync(int pageNumber, int pageSize)
    {
        var requestsTasks = Enumerable.Range(pageNumber, pageSize)
            .Select(CallTvMazeAsync)
            .ToList();
        var tvMazeResponses = await Task.WhenAll(requestsTasks);
        return tvMazeResponses.Select(ToShowResponse);
    }

    private static ShowResponse ToShowResponse(Show show)
    {
        return new ShowResponse
        {
            Id = show.Id,
            Name = show.Name,
            Cast = show.Embedded.Cast.Select(c => new Cast
            {
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