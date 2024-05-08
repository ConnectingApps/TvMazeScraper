using System.Threading.Tasks;
using Refit;

namespace TvMaze.Client
{
    public interface IMazeApi
    {
        [Get("/shows/{id}?embed={embed}")]
        Task<ApiResponse<Show>> GetShowWithDetailsAsync(int id, string embed);
    }
}