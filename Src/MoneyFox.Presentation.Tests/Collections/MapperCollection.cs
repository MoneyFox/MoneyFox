using AutoMapper;
using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.Infrastructure;
using Xunit;

namespace MoneyFox.Presentation.Tests.Collections
{
    [ExcludeFromCodeCoverage]
    [CollectionDefinition("AutoMapperCollection")]
    public class MapperCollection : ICollectionFixture<MapperCollectionFixture>
    {
    }

    public class MapperCollectionFixture : IDisposable
    {
        public IMapper mapper { get; set; }

        public MapperCollectionFixture() => mapper = AutoMapperFactory.Create();

        public void Dispose() => mapper = null;
    }
}
