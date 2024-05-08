namespace TvMazeScraper.Api.InternalModels;

public class ShowResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Cast[] Cast { get; set; } = null!;
}