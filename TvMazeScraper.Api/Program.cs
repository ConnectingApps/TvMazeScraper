using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using TvMaze.Client;
using TvMazeScraper.Api.EF;

namespace TvMazeScraper.Api;

using Refit;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("MazeDatabase");

        // Add services to the container.

        //NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
        var dataSourceBuilder = new NpgsqlDataSourceBuilder
        {
            ConnectionStringBuilder = { ConnectionString = connectionString}
        };

        dataSourceBuilder.EnableDynamicJson();  // Enable dynamic JSON

        // Register the custom data source with Npgsql
        var dataSource = dataSourceBuilder.Build();
        
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IDataAccess, DataAccess>();
        var baseUrl = builder.Configuration.GetValue<string>("BaseUrl")!;
        builder.Services.AddScoped<IShowContentRepository, ShowContentRepository>();
        
        var maxCalls = builder.Configuration.GetValue<int>("MaxCalls")!;
        var maxSeconds = builder.Configuration.GetValue<int>("MaxSeconds")!;
        var baseRateLimitPolicy = Policy.RateLimitAsync(maxCalls, TimeSpan.FromSeconds(maxSeconds));
        var rateLimitPolicy = baseRateLimitPolicy.AsAsyncPolicy<HttpResponseMessage>();
        
        builder.Services.AddRefitClient<IMazeApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl))
            .AddPolicyHandler(rateLimitPolicy); 

        builder.Services.AddDbContext<MazeContext>(options =>
            options.UseNpgsql(dataSource));
        
        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MazeContext>();
            dbContext.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}