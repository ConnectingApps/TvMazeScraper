using Refit;
using TvMaze.Client;

Console.WriteLine("Hello, World!");

var api = RestService.For<IMazeApi>("https://api.tvmaze.com");
var response = await api.GetShowWithDetailsAsync(1, "cast");
Console.WriteLine($"StatusCode response {response.StatusCode}");
Console.WriteLine($"{response.Content!.Embedded.Cast.Length} Cast Members");
