using Microsoft.Extensions.Logging;

namespace App.Lib.Database;

public class DistributedLockFactory
{
    private const double DefaultLockTimeout = 60;
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<DistributedLock> _logger;

    public DistributedLockFactory(DatabaseContext dbContext, ILogger<DistributedLock> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public Task<IDisposable> Lock(
        string name,
        CancellationToken cancellationToken = default)
    {
        return Lock(name, TimeSpan.FromSeconds(DefaultLockTimeout), cancellationToken);
    }

    public async Task<IDisposable> Lock(
        string name,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        var lockObj = new DistributedLock(_dbContext, _logger, name, timeout);
        await lockObj.Lock(cancellationToken);
        return lockObj;
    }
}