using System.Diagnostics.CodeAnalysis;
using App.Lib.Tests.Logging;
using Xunit.Abstractions;

namespace App.Lib.Database.Tests;

public class DistributedLockTest : DatabaseTest
{
    private const string TestLockName = "test-lock";
    private readonly DistributedLockFactory _lockFactory;

    public DistributedLockTest(ITestOutputHelper testOutputHelper)
    {
        _lockFactory = new DistributedLockFactory(
            _databaseContext,
            XUnitLogger.CreateLogger<DistributedLock>(testOutputHelper));
    }

    [Fact]
    [SuppressMessage("Usage", "CS1998")]
    public async void Lock_Once()
    {
        var lockResult = new List<string>();

        async Task FirstLock()
        {
            using (await _lockFactory.Lock(TestLockName))
            {
                lockResult.Add("first-before-sleep");
                await Task.Delay(500);
                lockResult.Add("first-after-sleep");
            }
        }

        async Task SecondLock()
        {
            await Task.Delay(100);
            using (await _lockFactory.Lock(TestLockName))
            {
                lockResult.Add("second-before-sleep");
                await Task.Delay(500);
                lockResult.Add("second-after-sleep");
            }
        }

        await Task.WhenAll(FirstLock(), SecondLock());
        lockResult.Should().Equal(new List<string>()
        {
            "first-before-sleep",
            "first-after-sleep",
            "second-before-sleep",
            "second-after-sleep",
        });
    }

    [Fact]
    public async void Lock_Timeout()
    {

    }

}