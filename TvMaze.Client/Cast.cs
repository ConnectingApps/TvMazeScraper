namespace TvMaze.Client
{
    public class Cast
    {
        public Person Person { get; set; } = null!;
        public Character Character { get; set; } = null!;
        public bool Self { get; set; }
        public bool Voice { get; set; }
    }
}