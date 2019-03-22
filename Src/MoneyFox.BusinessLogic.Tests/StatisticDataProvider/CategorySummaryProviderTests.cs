using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using Xunit;

namespace MoneyFox.BusinessLogic.Tests.StatisticDataProvider
{
    [ExcludeFromCodeCoverage]
    public class CategorySummaryProviderTests
    {
        [Fact]
        public async void GetValues_NullDependency_NullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(
                () => new CategorySummaryDataProvider(null).GetValues(DateTime.Today, DateTime.Today));
        }
    }
}