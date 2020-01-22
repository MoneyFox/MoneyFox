using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.Persistence;
using Xunit;

namespace MoneyFox.Application.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class QueryTestFixture : IDisposable
    {
        public EfCoreContext Context { get; }

        public QueryTestFixture()
        {
            Context = InMemoryEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            InMemoryEfCoreContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture>
    { }
}
