namespace Soenneker.Tests.HostedUnit.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class HostedUnitTestTests : HostedUnitTest
{
    public HostedUnitTestTests(Host host) : base(host)
    {

    }

    [Test]
    public void Default()
    {

    }
}