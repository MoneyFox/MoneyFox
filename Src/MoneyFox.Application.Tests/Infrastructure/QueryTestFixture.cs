using MoneyFox.Infrastructure.Persistence;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Application.Tests.Infrastructure;

[ExcludeFromCodeCoverage]
public class QueryTestFixture : IDisposable
{
    public QueryTestFixture()
    {
        Context = InMemoryEfCoreContextFactory.Create();
    }

    public EfCoreContext Context { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(Context);
}

[CollectionDefinition("QueryCollection")]
public class QueryCollection : ICollectionFixture<QueryTestFixture>
{
}