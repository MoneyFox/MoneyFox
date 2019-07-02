using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using Xunit;

namespace MoneyFox.BusinessLogic.Tests.StatisticDataProvider
{
    [ExcludeFromCodeCoverage]
    public class CategorySummaryProviderTests
    {
        [Fact]
        public async Task GetValues_NullDependency_NullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(
                () => new CategorySummaryDataProvider(null).GetValues(DateTime.Today, DateTime.Today));
        }
    }
}