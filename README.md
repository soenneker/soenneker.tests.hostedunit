[![](https://img.shields.io/nuget/v/soenneker.tests.hostedunit.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.tests.hostedunit/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.tests.hostedunit/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.tests.hostedunit/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.tests.hostedunit.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.tests.hostedunit/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.tests.hostedunit/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.tests.hostedunit/actions/workflows/codeql.yml)

# Soenneker.Tests.HostedUnit

A TUnit base class for tests that resolve services from a shared `UnitTestHost` while owning a per-test dependency-injection scope.

## Installation

```bash
dotnet add package Soenneker.Tests.HostedUnit
```

## Define the host

```csharp
using Microsoft.Extensions.DependencyInjection;
using Soenneker.TestHosts.Unit;

public sealed class Host : UnitTestHost
{
    public override Task InitializeAsync()
    {
        Services.AddSingleton<IClock, TestClock>();
        Services.AddScoped<OrderService>();

        return base.InitializeAsync();
    }
}
```

## Define the tests

```csharp
using Soenneker.Tests.HostedUnit;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class OrderServiceTests : HostedUnitTest
{
    public OrderServiceTests(Host host) : base(host)
    {
    }

    [Test]
    public async Task Creates_an_order()
    {
        OrderService service = Resolve<OrderService>(scoped: true);
        CreateOrder request = AutoFaker.Generate<CreateOrder>();

        Order result = await service.Create(request);

        await Assert.That(result.Id).IsNotEqualTo(Guid.Empty);
    }
}
```

`Resolve<T>()` resolves from the shared host's root provider. `Resolve<T>(scoped: true)` lazily creates one async scope for the test instance, reuses it for subsequent scoped resolutions, and disposes it after the test. `CreateScope()` can establish that scope explicitly and is idempotent while it exists.

The base class reuses the host's `AutoFaker`. Its `Logger` is resolved through the test scope, so log output follows the logging services configured by the host.

## Background work

If the host registers `IBackgroundQueue`, `WaitOnQueueToEmpty(cancellationToken)` waits for its queued work to finish before assertions or teardown continue. Supply a bounded cancellation token so a stuck producer cannot hang the test run. Accessing this method without an `IBackgroundQueue` registration fails service resolution.

The `ClassDataSource` should be shared so TUnit initializes and disposes the host once while each test instance still owns and disposes its own scope.
