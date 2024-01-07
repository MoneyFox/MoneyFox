namespace MoneyFox.Ui.Tests.Views.Statistics.CategorySummary;

using System.Collections.Immutable;
using Common.Navigation;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Ui.Views.Statistics.CategorySummary;

public sealed class PaymentForCategoryListViewModelTests
{
    [Fact]
    public void ReturnsCorrectExpenseAndRevenue_WhenGroupsAreEmpty()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var navigationService = Substitute.For<INavigationService>();
        var vm = new PaymentForCategoryListViewModel(mediator: mediator, navigationService: navigationService);

        // Act / Assert
        vm.TotalRevenue.Should().Be(0);
        vm.TotalExpenses.Should().Be(0);
    }

    [Fact]
    public async Task ReturnsCorrectExpenseAndRevenue_ForGroups()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var navigationService = Substitute.For<INavigationService>();
        var vm = new PaymentForCategoryListViewModel(mediator: mediator, navigationService: navigationService);
        mediator.Send(Arg.Any<GetPaymentsForCategorySummary.Query>())
            .Returns(
                ImmutableList.Create<GetPaymentsForCategorySummary.PaymentData>(
                    new(
                        Id: 1,
                        ChargedAccountId: 10,
                        Amount: 10,
                        CategoryName: null,
                        Date: DateOnly.MinValue,
                        Note: null,
                        IsCleared: false,
                        IsRecurring: false,
                        Type: PaymentType.Income),
                    new(
                        Id: 1,
                        ChargedAccountId: 10,
                        Amount: 20,
                        CategoryName: null,
                        Date: DateOnly.MinValue,
                        Note: null,
                        IsCleared: false,
                        IsRecurring: false,
                        Type: PaymentType.Expense)));

        // Act
        await vm.OnNavigatedAsync(new PaymentsForCategoryParameter(CategoryId: null, StartDate: DateTime.Today, EndDate: DateTime.Today));

        // Assert
        vm.TotalRevenue.Should().Be(10);
        vm.TotalExpenses.Should().Be(20);
    }
}
