using AutoMapper;
using MoneyFox.Application.Infrastructure;
using System;
using Xunit;

namespace MoneyFox.Presentation.Tests.Collections
{
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
