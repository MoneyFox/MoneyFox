namespace MoneyFox.Ui.Tests.Views.Statistics.CategorySummary;

using AutoMapper;
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
        var mapper = Substitute.For<IMapper>();
        var navigationService = Substitute.For<INavigationService>();
        var vm = new PaymentForCategoryListViewModel(mediator: mediator, mapper: mapper, navigationService: navigationService);

        // Act / Assert
        vm.TotalRevenue.Should().Be(0);
        vm.TotalExpenses.Should().Be(0);
    }

    [Fact]
    public async Task ReturnsCorrectExpenseAndRevenue_ForGroups()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var mapper = Substitute.For<IMapper>();
        var navigationService = Substitute.For<INavigationService>();
        var vm = new PaymentForCategoryListViewModel(mediator: mediator, mapper: mapper, navigationService: navigationService);
        mediator.Send(Arg.Any<GetPaymentsForCategorySummary.Query>()).Returns(new List<Payment>());
        mapper.Map<List<PaymentListItemViewModel>>(Arg.Any<List<Payment>>())
            .Returns(
                new List<PaymentListItemViewModel>
                {
                    new() { Amount = 10, Type = PaymentType.Income },
                    new() { Amount = 20, Type = PaymentType.Expense },
                    new() { Amount = 30, Type = PaymentType.Transfer }
                });

        // Act
        await vm.OnNavigatedAsync(new PaymentsForCategoryParameter(CategoryId: null, StartDate: DateTime.Today, EndDate: DateTime.Today));

        // Assert
        vm.TotalRevenue.Should().Be(10);
        vm.TotalExpenses.Should().Be(20);
    }
}
