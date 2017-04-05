using MoneyFox.Business.Tests.Fixtures;
using Xunit;

namespace MoneyFox.Business.Tests.TestCollections
{
    [CollectionDefinition("MvxIocCollection")]
    public class MvxIocCollection : ICollectionFixture<MvxIocFixture>
    {
    }
}
