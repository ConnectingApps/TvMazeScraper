namespace TvMazeScraper.Api;

public class RateLimitedHttpMessageHandler : DelegatingHandler
{
    private readonly SemaphoreSlim _semaphore;
    private readonly Timer _timer;
    private readonly int _maxCalls;
    private int _acquiredCount;

    public RateLimitedHttpMessageHandler(int maxCalls, TimeSpan timeSpan)
    {
        _maxCalls = maxCalls;
        _semaphore = new SemaphoreSlim(maxCalls, maxCalls);
        _acquiredCount = 0;

        // Timer to periodically reset the semaphore capacity
        _timer = new Timer(ReleaseSemaphoreSlots!, null, timeSpan, timeSpan);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);
        Interlocked.Increment(ref _acquiredCount);
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            _semaphore.Release();
            Interlocked.Decrement(ref _acquiredCount);
        }
    }

    private void ReleaseSemaphoreSlots(object state)
    {
        // Ensure we do not release more slots than have been acquired
        int currentCount = _semaphore.CurrentCount;
        int slotsToRelease = _maxCalls - currentCount;
        int safeReleaseCount = Math.Min(slotsToRelease, _acquiredCount);

        if (safeReleaseCount > 0)
        {
            _semaphore.Release(safeReleaseCount);
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