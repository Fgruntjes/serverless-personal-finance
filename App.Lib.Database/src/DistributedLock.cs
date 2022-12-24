using App.Lib.Database.Document;
using App.Lib.Database.Exception;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace App.Lib.Database;

public class DistributedLock : IDisposable
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<DistributedLock> _logger;
    private readonly string _name;
    private readonly TimeSpan _timeout;

    private IMongoCollection<DistributedLockDocument> LockCollection => _dbContext.GetCollection<DistributedLockDocument>();

    public DistributedLock(DatabaseContext dbContext, ILogger<DistributedLock> logger, string name, TimeSpan timeout)
    {
        _dbContext = dbContext;
        _logger = logger;
        _name = name;
        _timeout = timeout;
    }

    public async Task Lock(CancellationToken cancellationToken = default)
    {
        var expiresAt = DateTime.UtcNow.Add(_timeout);
        do
        {
            try
            {
                _logger.LogTrace("Trying lock {name}", _name);
                // Delete expired locks
                await LockCollection.DeleteManyAsync(
                    l => l.ExpiresAt < DateTime.UtcNow,
                    cancellationToken: cancellationToken);

                // Try get current lock
                await LockCollection.InsertOneAsync(new DistributedLockDocument
                {
                    Name = _name,
                    ExpiresAt = expiresAt.ToUniversalTime()
                }, cancellationToken: cancellationToken);

                _logger.LogTrace("Created lock {name}", _name);
                return;
            }
            catch (MongoWriteException exception)
            {
                if (exception.WriteError.Category != ServerErrorCategory.DuplicateKey)
                {
                    throw;
                }

                // Wait and retry
                var delay = (int)Math.Ceiling(_timeout.TotalMilliseconds / 60);
                _logger.LogTrace("Existing lock {name}, waiting for {delay}ms", _name, delay);
                await Task.Delay(delay, cancellationToken);
            }
        } while (expiresAt > DateTime.UtcNow);

        _logger.LogTrace("Create lock {name} expired after {timeout}", _name, _timeout);
        throw new LockTimeoutException($"Acquire lock {_name} timed out after {_timeout:g}");
    }

    private async void Unlock()
    {
        await LockCollection.DeleteManyAsync(filter => filter.Name == _name);
        _logger.LogTrace("Lock {name} released", _name, _timeout);
    }

    public void Dispose()
    {
        Unlock();
    }
}