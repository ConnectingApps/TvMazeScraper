using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using TvMazeScraper.Api;

namespace TvScraper.Api.IntegrationTest;

public class ShowResponseTest : IDisposable
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _client;
    private readonly IShowApi _showApi;

    public ShowResponseTest()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _client = _webApplicationFactory.CreateClient();
        _showApi =  RestService.For<IShowApi>(_client);
    }
    
    [Fact]
    public async Task GetShowDataAsyncTest()
    {
        var response = await _showApi.GetShowDataAsync(1, 1);
        (response.StatusCode, response.Error?.Content).Should().Be((HttpStatusCode.OK, null));
    }

    public void Dispose()
    {
        _webApplicationFactory.Dispose();
        _client.Dispose();
    }
}