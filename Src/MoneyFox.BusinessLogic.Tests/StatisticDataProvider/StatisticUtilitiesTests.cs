using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using MoneyFox.Foundation.Models;
using Should;
using Xunit;

namespace MoneyFox.BusinessLogic.Tests.StatisticDataProvider
{
    [ExcludeFromCodeCoverage]
    public class StatisticUtilitiesTests
    {
        [Theory]
        [InlineData(3.234, 3.23)]
        [InlineData(6.589, 6.59)]
        [InlineData(55.385, 55.39)]
        [InlineData(9, 9)]
        public void RoundStatisticItems_ListOfItems_ListWithRoundedPercentages(double value, double result)
        {
            var statisticItems = new List<CategoryOverviewItem>
            {
                new CategoryOverviewItem
                {
                    Percentage = value
                }
            };
            StatisticUtilities.RoundStatisticItems(statisticItems);
            statisticItems[0].Percentage.ShouldEqual(result);
        }
    }
}
