namespace MoneyFox.Ui.Tests.Views.Statistics.CategorySummary;

using Domain.Aggregates.AccountAggregate;
using FluentAssertions;
using Ui.Views.Statistics.CategorySummary;
using Xunit;

public sealed class PaymentDayGroupTests
{
    [Fact]
    public void CalculatesTotalExpenseAndRevenueCorrectly_WhenGroupContainsNoItems()
    {
        // Arrange
        var group = new PaymentDayGroup(DateOnly.FromDateTime(DateTime.Today), new List<PaymentListItemViewModel>());

        // Act / Assert
        group.TotalExpense.Should().Be(0);
        group.TotalRevenue.Should().Be(0);
    }
    
    [Fact]
    public void CalculatesTotalExpenseAndRevenueCorrectly_WithTransferInList()
    {
        // Arrange
        var paymentListItemViewModels = new List<PaymentListItemViewModel>
        {
            new PaymentListItemViewModel { Amount = 10, Type = PaymentType.Expense },
            new PaymentListItemViewModel { Amount = 20, Type = PaymentType.Income },
            new PaymentListItemViewModel { Amount = 30, Type = PaymentType.Transfer }
        };
        var group = new PaymentDayGroup(DateOnly.FromDateTime(DateTime.Today), paymentListItemViewModels);

        // Act / Assert
        group.TotalExpense.Should().Be(10);
        group.TotalRevenue.Should().Be(20);
    }
}
