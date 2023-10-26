namespace MoneyFox.Ui.Tests.Views.Statistics.CategorySummary;

using Domain.Aggregates.AccountAggregate;
using Ui.Views.Statistics.CategorySummary;

public sealed class PaymentDayGroupTests
{
    [Fact]
    public void CalculatesTotalExpenseAndRevenueCorrectly_WhenGroupContainsNoItems()
    {
        // Arrange
        var group = new PaymentDayGroup(date: DateOnly.FromDateTime(DateTime.Today), payments: new List<PaymentListItemViewModel>());

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
            new() { Amount = 10, Type = PaymentType.Expense },
            new() { Amount = 20, Type = PaymentType.Income },
            new() { Amount = 30, Type = PaymentType.Transfer }
        };

        var group = new PaymentDayGroup(date: DateOnly.FromDateTime(DateTime.Today), payments: paymentListItemViewModels);

        // Act / Assert
        group.TotalExpense.Should().Be(10);
        group.TotalRevenue.Should().Be(20);
    }
}
