namespace TvMazeScraper.Api;

public class RateLimitedHttpMessageHandler : DelegatingHandler
{
    private readonly SemaphoreSlim _semaphore;
    private readonly Timer _timer;
    private readonly int _maxCalls;

    public RateLimitedHttpMessageHandler(int maxCalls, TimeSpan timeSpan)
    {
        _maxCalls = maxCalls;
        _semaphore = new SemaphoreSlim(maxCalls, maxCalls);

        // Timer to periodically reset the semaphore capacity
        _timer = new Timer(ReleaseSemaphoreSlots!, null, timeSpan, timeSpan);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void ReleaseSemaphoreSlots(object state)
    {
        int slotsToRelease = _maxCalls - _semaphore.CurrentCount;
        if (slotsToRelease > 0)
        {
            _semaphore.Release(slotsToRelease);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _timer.Dispose();
            _semaphore.Dispose();
        }
        base.Dispose(disposing);
    }
}