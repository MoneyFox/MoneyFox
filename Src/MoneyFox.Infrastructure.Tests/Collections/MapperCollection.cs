using System;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Xunit;

namespace MoneyFox.Infrastructure.Tests.Collections
{
    [ExcludeFromCodeCoverage]
    [CollectionDefinition("AutoMapperCollection")]
    public class MapperCollection : ICollectionFixture<MapperCollectionFixture>
    { }

    [ExcludeFromCodeCoverage]
    public class MapperCollectionFixture : IDisposable
    {
        public IMapper Mapper { get; set; }

        public MapperCollectionFixture()
        {
            Mapper = AutoMapperFactory.Create();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Mapper = null;
        }
    }
}
