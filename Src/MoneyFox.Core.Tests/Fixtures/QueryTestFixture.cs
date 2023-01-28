namespace MoneyFox.Core.Tests.Fixtures;

using System.Diagnostics.CodeAnalysis;
using MoneyFox.Infrastructure.Persistence;


public class QueryTestFixture : IDisposable
{
    public QueryTestFixture()
    {
        Context = InMemoryAppDbContextFactory.Create();
    }

    public AppDbContext Context { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        InMemoryAppDbContextFactory.Destroy(Context);
    }
}

[CollectionDefinition("QueryCollection")]
public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
