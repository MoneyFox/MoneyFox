using MoneyFox.Persistence;
using System;
using Xunit;

namespace MoneyFox.Application.Tests.Infrastructure
{
    public class QueryTestFixture : IDisposable
    {
        public EfCoreContext Context { get; private set; }
        public QueryTestFixture()
        {
            Context = EfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            EfCoreContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
