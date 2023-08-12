namespace MoneyFox.Ui.Tests.Views.Statistics.CategorySummary;

using AutoMapper;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Ui.Views.Statistics.CategorySummary;
using Xunit;

public sealed class PaymentForCategoryListViewModelTests
{
    [Fact]
    public void ReturnsCorrectExpenseAndRevenue_WhenGroupsAreEmpty()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var mapper = Substitute.For<IMapper>();
        var vm = new PaymentForCategoryListViewModel(mediator: mediator, mapper: mapper);

        // Act / Assert
        vm.TotalRevenue.Should().Be(0);
        vm.TotalExpenses.Should().Be(0);
    }

    [Fact]
    public void ReturnsCorrectExpenseAndRevenue_ForGroups()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var mapper = Substitute.For<IMapper>();
        var vm = new PaymentForCategoryListViewModel(mediator: mediator, mapper: mapper);
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
        vm.Receive(new(categoryId: null, startDate: DateTime.Today, endDate: DateTime.Today));

        // Assert
        vm.TotalRevenue.Should().Be(10);
        vm.TotalExpenses.Should().Be(20);
    }
}
