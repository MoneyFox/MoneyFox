using MoneyFox.ServiceLayer.Tests.Fixtures;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.TestCollections
{
    [CollectionDefinition("MvxIocCollection")]
    public class MvxIocCollection : ICollectionFixture<MvxIocFixture>
    {
    }
}
