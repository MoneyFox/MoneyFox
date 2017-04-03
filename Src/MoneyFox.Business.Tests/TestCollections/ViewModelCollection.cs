using MoneyFox.Business.Tests.Fixtures;
using Xunit;

namespace MoneyFox.Business.Tests.TestCollections
{
    [CollectionDefinition("ViewModel collection")]
    public class ViewModelCollection : ICollectionFixture<MvxIocFixture>
    {
    }
}
