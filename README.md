[![](https://img.shields.io/nuget/v/soenneker.tests.hostedunit.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.tests.hostedunit/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.tests.hostedunit/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.tests.hostedunit/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.tests.hostedunit.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.tests.hostedunit/)

# Soenneker.Tests.HostedUnit

A hosted test that provides synthetic inversion of control via `TestHost`. Its most used function is `Resolve{T}`, which retrieves a service from the host service provider.

## Install

```bash
dotnet add package Soenneker.Tests.HostedUnit
```

## Quick start

```csharp
using Soenneker.Tests.HostedUnit.Abstract;

IHostedUnitTest hostedUnitTest = /* resolve from DI */;
var result = hostedUnitTest.Resolve();
```

Resolves a service from the host service provider.

## What you get

- `IHostedUnitTest` — A hosted test that provides synthetic inversion of control via `TestHost`. Its most used function is `Resolve{T}`, which retrieves a service from the host service provider.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IHostedUnitTest.Resolve(scoped)` | Resolves a service from the host service provider. | The resulting value. |
| `IHostedUnitTest.CreateScope()` | Creates a scope for resolving scoped services. | Usually you will want to use `Resolve{T}` instead. |
| `IHostedUnitTest.WaitOnQueueToEmpty(cancellationToken)` | Checks the background queue until it is empty. | A task that completes when the wait on queue to empty operation is complete. |

## Important behavior

- `IHostedUnitTest.Resolve(scoped)`: Optionally creates a scope if needed, if one does not already exist.
- `IHostedUnitTest.CreateScope()`: Usually you will want to use `Resolve{T}` instead.

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
- Dispose instances you own when their scope ends so held resources can be released.
