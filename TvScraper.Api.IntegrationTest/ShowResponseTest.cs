using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using TvMazeScraper.Api;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace TvScraper.Api.IntegrationTest;

public class ShowResponseTest : IDisposable
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _client;
    private readonly IShowApi _showApi;
    private readonly string _tvMazeResponseShow1;
    private readonly WireMockServer _wireMockServer;

    public ShowResponseTest()
    {
        _wireMockServer = WireMockServer.Start();
        var urlOfMockServer = _wireMockServer.Url!;
        Environment.SetEnvironmentVariable("BaseUrl", urlOfMockServer, EnvironmentVariableTarget.Process);
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _client = _webApplicationFactory.CreateClient();
        _showApi =  RestService.For<IShowApi>(_client);
        _tvMazeResponseShow1 = File.ReadAllText(Path.Combine("TestData", "show1.json"));
    }

    private void ConfigureServer(int statusCode, string jsonResponse)
    {
        _wireMockServer.Given(Request.Create().UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(statusCode) // HTTP status code 200
                    .WithBody(jsonResponse) // Response body with the long JSON string
                    .WithHeader("Content-Type", "application/json") // Content-Type header
            );
    }
    
    [Fact]
    public async Task GetShowDataAsyncTest()
    {
        var now = DateTime.Now;
        var idToUse = int.Parse($"{now.Hour}{now.Minute}{now.Second}{now.Millisecond}");
        ConfigureServer(200, _tvMazeResponseShow1);
        var response = await _showApi.GetShowDataAsync(idToUse, 1);
        (response.StatusCode, 
            response.Error?.Content,
            _wireMockServer.LogEntries.Count(),
            response.Content?.FirstOrDefault()?.Cast.Length
            ).Should().Be((HttpStatusCode.OK, null, 1, 15));
        _wireMockServer.LogEntries.Select(l => l.RequestMessage.Url).First().Should().
            Contain(idToUse.ToString());
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetShowDataAsyncRangeTest(int pageSize)
    {
        var now = DateTime.Now;
        var idToUse = int.Parse($"{now.Hour}{now.Minute}{now.Second}{now.Millisecond}");
        ConfigureServer(200, _tvMazeResponseShow1);
        var response = await _showApi.GetShowDataAsync(idToUse, pageSize);
        (response.StatusCode, 
                response.Content?.Count,
                response.Error?.Content).Should()
            .Be((HttpStatusCode.OK, pageSize, null));
    }

    [Theory]
    [InlineData(1,-1)]
    [InlineData(1,0)]
    [InlineData(-1,1)]
    public async Task GetShowDataBadRequestTest(int pageNumber, int pageSize)
    {
        ConfigureServer(200, _tvMazeResponseShow1);
        var response = await _showApi.GetShowDataAsync(pageNumber, pageSize);
        (response.StatusCode, _wireMockServer.LogEntries.Count()).Should()
            .Be((HttpStatusCode.BadRequest, 0));
    }
    
    [Fact]
    public async Task GetShowDataTwoTimesAsyncTest()
    {
        var now = DateTime.Now;
        var idToUse = int.Parse($"{now.Hour}{now.Minute}{now.Second}{now.Millisecond}");
        ConfigureServer(200, _tvMazeResponseShow1);
        var responseA = await _showApi.GetShowDataAsync(idToUse, 1);
        var responseB = await _showApi.GetShowDataAsync(idToUse, 1);

        (responseA.StatusCode,
            responseB.StatusCode,
            _wireMockServer.LogEntries.Count()).
            Should().Be((HttpStatusCode.OK, HttpStatusCode.OK, 1));
    }
    
    public void Dispose()
    {
        _webApplicationFactory.Dispose();
        _client.Dispose();
    }
}