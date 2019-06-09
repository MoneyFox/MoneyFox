using System.Diagnostics.CodeAnalysis;
using MoneyFox.Presentation.Tests.Fixtures;
using Xunit;

namespace MoneyFox.Presentation.Tests.TestCollections
{
    [CollectionDefinition("MvxIocCollection")]
    [ExcludeFromCodeCoverage]
    public class MvxIocCollection : ICollectionFixture<MvxIocFixture>
    {
    }
}
