using System.Text.Json.Serialization;

namespace TvMaze.Client
{
    public class Person
    {
        public int Id  { get; set; }
        public string Url { get; set; } = null!;
        public string Name { get; set; } = null!;
        public Country Country { get; set; } = null!;
        public string? Birthday { get; set; } = null;
        public object Deathday { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public Image Image { get; set; } = null!;
        public int? Updated { get; set; } 
        [JsonPropertyName("_links")]
        public Links Links { get; set; } = null!;
    }
}