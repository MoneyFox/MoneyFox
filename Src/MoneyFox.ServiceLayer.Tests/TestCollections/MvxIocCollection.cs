using System.Diagnostics.CodeAnalysis;
using MoneyFox.ServiceLayer.Tests.Fixtures;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.TestCollections
{
    [CollectionDefinition("MvxIocCollection")]
    [ExcludeFromCodeCoverage]
    public class MvxIocCollection : ICollectionFixture<MvxIocFixture>
    {
    }
}
