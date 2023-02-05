namespace MoneyFox.Ui.Tests.Views.Budget;

using System.Collections.Immutable;
using Core.Queries.BudgetListLoading;
using Domain.Tests.TestFramework;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Ui.Views.Budget;
using Xunit;

public abstract class BudgetListPageViewModelTests
{
    private readonly ISender sender = Substitute.For<ISender>();
    private readonly BudgetListViewModel viewModel;

    protected BudgetListPageViewModelTests()
    {
        viewModel = new(sender);
    }

    private static void AssertBudgetListViewModel(BudgetListItemViewModel actualBudgetVm, TestData.IBudget expectedBudgetData)
    {
        actualBudgetVm.Id.Should().Be(expectedBudgetData.Id);
        actualBudgetVm.Name.Should().Be(expectedBudgetData.Name);
        actualBudgetVm.SpendingLimit.Should().Be(expectedBudgetData.SpendingLimit);
        actualBudgetVm.CurrentSpending.Should().Be(expectedBudgetData.CurrentSpending);
    }

    [Collection(nameof(BudgetListPageViewModelTests))]
    public class WithNoBudgetsAvailable : BudgetListPageViewModelTests
    {
        [Fact]
        public async Task InitializeBudgetsCollectionEmpty_WhenNotItemsFound()
        {
            // Act
            await viewModel.InitializeCommand.ExecuteAsync(null);

            // Assert
            viewModel.Budgets.Should().BeEmpty();
        }
    }

    [Collection(nameof(BudgetListPageViewModelTests))]
    public class WithBudgetAvailable : BudgetListPageViewModelTests
    {
        private readonly TestData.DefaultBudget budgetTestData;

        public WithBudgetAvailable()
        {
            budgetTestData = new();
            sender.Send(Arg.Any<LoadBudgetListData.Query>())
                .Returns(
                    ImmutableList.Create(
                        new BudgetListData(
                            id: budgetTestData.Id,
                            name: budgetTestData.Name,
                            spendingLimit: budgetTestData.SpendingLimit,
                            currentSpending: budgetTestData.CurrentSpending)));
        }

        [Fact]
        public async Task InitializeBudgetsCollection_WithLoadedBudgets()
        {
            // Act
            await viewModel.InitializeCommand.ExecuteAsync(null);

            // Assert
            viewModel.Budgets.Should().HaveCount(1);
            var loadedBudget = viewModel.Budgets.Single();
            AssertBudgetListViewModel(actualBudgetVm: loadedBudget, expectedBudgetData: budgetTestData);
        }

        [Fact]
        public async Task DoesNotAddEntriesTwice_WhenInitializeIsCalledMultipleTimes()
        {
            // Act
            await viewModel.InitializeCommand.ExecuteAsync(null);
            await viewModel.InitializeCommand.ExecuteAsync(null);

            // Assert
            viewModel.Budgets.Should().HaveCount(1);
            var loadedBudget = viewModel.Budgets.Single();
            AssertBudgetListViewModel(actualBudgetVm: loadedBudget, expectedBudgetData: budgetTestData);
        }
    }

    [Collection(nameof(BudgetListPageViewModelTests))]
    public class WithMultipleBudgetAvailable : BudgetListPageViewModelTests
    {
        public WithMultipleBudgetAvailable()
        {
            TestData.DefaultBudget budgetTestData1 = new();
            _ = sender.Send(Arg.Any<LoadBudgetListData.Query>())
                .Returns(
                    ImmutableList.Create(
                        new BudgetListData(
                            id: budgetTestData1.Id,
                            name: "Beverages",
                            spendingLimit: budgetTestData1.SpendingLimit,
                            currentSpending: budgetTestData1.CurrentSpending),
                        new BudgetListData(
                            id: budgetTestData1.Id,
                            name: "Apples",
                            spendingLimit: budgetTestData1.SpendingLimit,
                            currentSpending: budgetTestData1.CurrentSpending)));
        }

        [Fact]
        public async Task InitializeBudgetsCollection_WithLoadedBudgetsCorrectlySorted()
        {
            // Act
            await viewModel.InitializeCommand.ExecuteAsync(null);

            // Assert
            _ = viewModel.Budgets.Should().HaveCount(2);
            _ = viewModel.Budgets[0].Name.Should().Be("Apples");
            _ = viewModel.Budgets[1].Name.Should().Be("Beverages");
        }

        [Fact]
        public async Task TotalAmountCorrectlyCalculated()
        {
            // Act
            await viewModel.InitializeCommand.ExecuteAsync(null);

            // Assert
            var expectedAmount = viewModel.Budgets.ToList().Sum(b => b.SpendingLimit);
            _ = viewModel.BudgetedAmount.Should().Be(expectedAmount);
        }
    }
}
