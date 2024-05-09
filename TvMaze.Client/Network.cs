namespace TvMaze.Client
{
    public class Network
    {
        public int Id  { get; set; }
        public string Name { get; set; } = null!;
        public Country Country { get; set; } = null!;
        public string OfficialSite { get; set; } = null!;
    }
}