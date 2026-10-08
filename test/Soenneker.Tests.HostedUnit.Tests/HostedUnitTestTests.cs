using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Soenneker.Tests.HostedUnit.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class HostedUnitTestTests : HostedUnitTest
{
    public HostedUnitTestTests(Host host) : base(host)
    {

    }

    [Test]
    public async ValueTask Default(CancellationToken cancellationToken)
    {
    }

    [Test]
    public async ValueTask Realtime_log(CancellationToken cancellationToken)
    {
        for (var x = 0; x < 10; x++)
        {
            Logger.LogInformation("Testing");
            await Task.Delay(1000, cancellationToken: cancellationToken);
        }
    }
}