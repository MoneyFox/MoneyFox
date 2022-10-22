namespace MoneyFox.Ui.Tests.ViewModels.Budget;

using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Core.Tests.TestFramework;
using FluentAssertions;
using MediatR;
using MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading;
using MoneyFox.Ui.ViewModels.Budget;
using NSubstitute;
using Xunit;

public class BudgetListPageViewModelShould
{
    private readonly ISender sender;
    private readonly BudgetListViewModel viewModel;

    protected BudgetListPageViewModelShould()
    {
        sender = Substitute.For<ISender>();
        viewModel = new BudgetListViewModel(sender);
    }

    private static void AssertBudgetListViewModel(BudgetListItemViewModel actualBudgetVm, TestData.IBudget expectedBudgetData)
    {
        _ = actualBudgetVm.Id.Should().Be(expectedBudgetData.Id);
        _ = actualBudgetVm.Name.Should().Be(expectedBudgetData.Name);
        _ = actualBudgetVm.SpendingLimit.Should().Be(expectedBudgetData.SpendingLimit);
        _ = actualBudgetVm.CurrentSpending.Should().Be(expectedBudgetData.CurrentSpending);
    }

    public class WithNoBudgetsAvailable : BudgetListPageViewModelShould
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

    public class WithBudgetAvailable : BudgetListPageViewModelShould
    {
        private readonly TestData.DefaultBudget budgetTestData;

        public WithBudgetAvailable()
        {
            budgetTestData = new TestData.DefaultBudget();
            _ = sender.Send(Arg.Any<LoadBudgetListData.Query>())
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
            _ = viewModel.Budgets.Should().HaveCount(1);
            BudgetListItemViewModel loadedBudget = viewModel.Budgets.Single();
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
            BudgetListItemViewModel loadedBudget = viewModel.Budgets.Single();
            AssertBudgetListViewModel(actualBudgetVm: loadedBudget, expectedBudgetData: budgetTestData);
        }
    }

    public class WithMultipleBudgetAvailable : BudgetListPageViewModelShould
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
            decimal expectedAmount = viewModel.Budgets.Sum(b => b.SpendingLimit);
            _ = viewModel.BudgetedAmount.Should().Be(expectedAmount);
        }
    }
}

