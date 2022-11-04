namespace MoneyFox.Ui.Tests.ViewModels.Budget;

using System.Collections.Immutable;
using Core.ApplicationCore.Queries.BudgetListLoading;
using Core.Tests.TestFramework;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Views.Budget;
using Xunit;

public class BudgetListPageViewModelTests
{
    private readonly ISender sender;
    private readonly BudgetListViewModel viewModel;

    protected BudgetListPageViewModelTests()
    {
        sender = Substitute.For<ISender>();
        viewModel = new(sender);
    }

    private static void AssertBudgetListViewModel(BudgetListItemViewModel actualBudgetVm, TestData.IBudget expectedBudgetData)
    {
        _ = actualBudgetVm.Id.Should().Be(expectedBudgetData.Id);
        _ = actualBudgetVm.Name.Should().Be(expectedBudgetData.Name);
        _ = actualBudgetVm.SpendingLimit.Should().Be(expectedBudgetData.SpendingLimit);
        _ = actualBudgetVm.CurrentSpending.Should().Be(expectedBudgetData.CurrentSpending);
    }

    public class WithNoBudgetsAvailable : BudgetListPageViewModelTests
    {
        [Fact]
        public async Task InitializeBudgetsCollectionEmpty_WhenNotItemsFound()
        {
            // Act
            await viewModel.InitializeCommand.ExecuteAsync(null);

            // Assert
            _ = viewModel.Budgets.Should().BeEmpty();
        }
    }

    public class WithBudgetAvailable : BudgetListPageViewModelTests
    {
        private readonly TestData.DefaultBudget budgetTestData;

        public WithBudgetAvailable()
        {
            budgetTestData = new();
            _ = sender.Send(Arg.Any<LoadBudgetListData.Query>())
                .Returns(
                    ImmutableList.Create(
                        new BudgetListData(
                            id: budgetTestData.Id,
                            name: budgetTestData.Name,
                            spendingLimit: budgetTestData.SpendingLimit,
                            currentSpending: budgetTestData.CurrentSpending,
                            budgetTestData.BudgetTimeRange)));
        }

        [Fact]
        public async Task InitializeBudgetsCollection_WithLoadedBudgets()
        {
            // Act
            await viewModel.InitializeCommand.ExecuteAsync(null);

            // Assert
            _ = viewModel.Budgets.Should().HaveCount(1);
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
            _ = viewModel.Budgets.Should().HaveCount(1);
            var loadedBudget = viewModel.Budgets.Single();
            AssertBudgetListViewModel(actualBudgetVm: loadedBudget, expectedBudgetData: budgetTestData);
        }
    }

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
                            currentSpending: budgetTestData1.CurrentSpending,
                            budgetTestData1.BudgetTimeRange),
                        new BudgetListData(
                            id: budgetTestData1.Id,
                            name: "Apples",
                            spendingLimit: budgetTestData1.SpendingLimit,
                            currentSpending: budgetTestData1.CurrentSpending,
                            budgetTestData1.BudgetTimeRange)));
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
            var expectedAmount = viewModel.Budgets.Sum(b => b.SpendingLimit);
            _ = viewModel.BudgetedAmount.Should().Be(expectedAmount);
        }
    }
}
