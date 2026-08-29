using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Tests.HostedUnit.Abstract;

/// <summary>
/// A hosted test that provides synthetic inversion of control via <c>TestHost</c>. <para/>
/// Its most used function is <see cref="Resolve{T}"/>, which retrieves a service from the host service provider.
/// </summary>
public interface IHostedUnitTest : IAsyncDisposable
{
    /// <summary>
    /// Resolves a service from the host service provider.
    /// </summary>
    /// <typeparam name="T">Type of value handled by the Hosted Unit Test.</typeparam>
    /// <param name="scoped">Whether scoped.</param>
    /// <returns>The resulting value.</returns>
    /// <remarks>
    /// Optionally creates a scope if needed, if one does not already exist.
    /// </remarks>
    T Resolve<T>(bool scoped = false) where T : notnull;

    /// <summary>
    /// Creates a scope for resolving scoped services.
    /// </summary>
    /// <remarks>
    /// Usually you will want to use <see cref="Resolve{T}"/> instead.
    /// </remarks>
    void CreateScope();

    /// <summary>
    /// Checks the background queue until it is empty.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the wait on queue to empty operation is complete.</returns>
    ValueTask WaitOnQueueToEmpty(CancellationToken cancellationToken = default);
}
