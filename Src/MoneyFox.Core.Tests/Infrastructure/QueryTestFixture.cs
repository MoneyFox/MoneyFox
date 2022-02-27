namespace MoneyFox.Core.Tests.Infrastructure
{
    using MoneyFox.Infrastructure.Persistence;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class QueryTestFixture : IDisposable
    {
        public AppDbContext Context { get; }

        public QueryTestFixture()
        {
            Context = InMemoryAppDbContextFactory.Create();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(Context);
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture>
    {
    }
}