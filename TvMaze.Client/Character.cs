using System.Text.Json.Serialization;

namespace TvMaze.Client
{
    public class Character
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public Image Image { get; set; }
        [JsonPropertyName("_links")]
        public Links Links { get; set; }
    }
}