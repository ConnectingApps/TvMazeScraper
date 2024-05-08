using System.Text.Json.Serialization;

namespace TvMaze.Client
{
    public class Character
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public string Name { get; set; } = null!;
        public Image Image { get; set; } = null!;
        [JsonPropertyName("_links")]
        public Links Links { get; set; } = null!;
    }
}