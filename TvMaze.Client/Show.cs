using System.Text.Json.Serialization;

namespace TvMaze.Client
{
    public class Show
    {
        public int Id  { get; set; } = 0;
        public string Url { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string[] Genres { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? Runtime { get; set; }
        public int? AverageRuntime { get; set; }
        public string Premiered { get; set; } = null!;
        public string Ended { get; set; } = null!;
        public string OfficialSite { get; set; } = null!;
        public Schedule Schedule { get; set; } = null!;
        public Rating Rating { get; set; } = null!;
        public int? Weight { get; set; }
        public Network Network { get; set; } = null!;
        public object WebChannel { get; set; } = null!;
        public object DvdCountry { get; set; } = null!;
        public Externals Externals { get; set; } = null!;
        public Image Image { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public int? Updated { get; set; }
        
        [JsonPropertyName("_links")]
        public Links Links { get; set; } = null!;
        
        [JsonPropertyName("_embedded")]
        public Embedded Embedded { get; set; } = null!;
    }
}