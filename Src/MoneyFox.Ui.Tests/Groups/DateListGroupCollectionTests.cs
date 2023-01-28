namespace MoneyFox.Ui.Tests.Groups;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Common.Groups;
using FluentAssertions;
using Views.Payments;
using Xunit;

[ExcludeFromCodeCoverage]
public class DateListGroupCollectionTests
{
    [Fact]
    public void CreateGroupReturnsCorrectGroup()
    {
        // Arrange
        List<PaymentViewModel> paymentList = new() { new() { Id = 1, Date = DateTime.Now }, new() { Id = 2, Date = DateTime.Now.AddMonths(-1) } };

        // Act
        var createdGroup = DateListGroupCollection<PaymentViewModel>.CreateGroups(
            items: paymentList,
            getKey: s => s.Date.ToString(format: "D", provider: CultureInfo.CurrentCulture),
            getSortKey: s => s.Date);

        // Assert
        _ = createdGroup.Should().HaveCount(2);
        createdGroup[0][0].Id.Should().Be(1);
        createdGroup[1][0].Id.Should().Be(2);
    }
}
