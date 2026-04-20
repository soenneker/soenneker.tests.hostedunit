using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Soenneker.Extensions.ServiceProvider;
using Soenneker.Extensions.ValueTask;
using Soenneker.TestHosts.Core;
using Soenneker.Tests.HostedUnit.Abstract;
using Soenneker.Tests.Logging;
using Soenneker.Tests.Unit;
using Soenneker.Utils.BackgroundQueue.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Tests.HostedUnit;

///<inheritdoc cref="IHostedUnitTest"/>
public abstract class HostedUnitTest : UnitTest, IHostedUnitTest
{
    public TestHost Host { get; }

    public AsyncServiceScope? Scope { get; private set; }

    private readonly Lazy<IBackgroundQueue> _backgroundQueue;

    protected HostedUnitTest(TestHost host) : base(host.AutoFaker, enableLogging: false)
    {
        Host = host ?? throw new ArgumentNullException(nameof(host));

        LazyLogger = new Lazy<ILogger<LoggingTest>>(() => Resolve<ILogger<HostedUnitTest>>(scoped: true), LazyThreadSafetyMode.ExecutionAndPublication);

        _backgroundQueue = new Lazy<IBackgroundQueue>(() => Resolve<IBackgroundQueue>(), LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public T Resolve<T>(bool scoped = false)
    {
        if (Host.ServicesProvider == null)
            throw new Exception($"ServiceProvider was null trying to resolve service {typeof(T).Name}! Not able to resolve service");

        if (!scoped)
            return Host.ServicesProvider.Get<T>();

        if (Scope == null)
            CreateScope();

        return Scope!.Value.ServiceProvider.Get<T>();
    }

    public void CreateScope()
    {
        if (Host.ServicesProvider == null)
            throw new Exception("ServiceProvider was null trying create a scope!");

        Scope = Host.ServicesProvider.CreateAsyncScope();
    }

    public ValueTask WaitOnQueueToEmpty(CancellationToken cancellationToken = default)
    {
        return _backgroundQueue.Value.WaitUntilEmpty(cancellationToken);
    }

    public override async ValueTask DisposeAsync()
    {
        if (Scope is not null)
        {
            await Scope.Value.DisposeAsync().NoSync();
            Scope = null;
        }

        await base.DisposeAsync().NoSync();
    }
}