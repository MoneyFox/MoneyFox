namespace MoneyFox.Ui.Tests.Views.Budget;

using System.Collections.Immutable;
using Common.Navigation;
using Core.Queries.BudgetList;
using Domain.Tests.TestFramework;
using MediatR;
using Ui.Views.Budget;

public abstract class BudgetListPageViewModelTests
{
    private readonly ISender sender = Substitute.For<ISender>();
    private readonly INavigationService navigationService = Substitute.For<INavigationService>();
    private readonly BudgetListViewModel viewModel;

    protected BudgetListPageViewModelTests()
    {
        viewModel = new(sender, navigationService);
    }

    private static void AssertBudgetListViewModel(BudgetListItemViewModel actualBudgetVm, TestData.IBudget expectedBudgetData)
    {
        actualBudgetVm.Id.Should().Be(expectedBudgetData.Id);
        actualBudgetVm.Name.Should().Be(expectedBudgetData.Name);
        actualBudgetVm.SpendingLimit.Should().Be(expectedBudgetData.SpendingLimit);
        actualBudgetVm.CurrentSpending.Should().Be(expectedBudgetData.CurrentSpending);
        actualBudgetVm.MonthlyBudget.Should().Be(expectedBudgetData.MonthlyBudget);
        actualBudgetVm.MonthlySpending.Should().Be(expectedBudgetData.CurrentSpending / expectedBudgetData.Interval);
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
            sender.Send(Arg.Any<LoadBudgetDataForList.Query>())
                .Returns(
                    ImmutableList.Create(
                        new BudgetData(
                            Id: budgetTestData.Id,
                            Name: budgetTestData.Name,
                            SpendingLimit: budgetTestData.SpendingLimit,
                            CurrentSpending: budgetTestData.CurrentSpending,
                            MonthlyBudget: budgetTestData.MonthlyBudget,
                            MonthlySpending: budgetTestData.CurrentSpending / budgetTestData.Interval)));
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
            _ = sender.Send(Arg.Any<LoadBudgetDataForList.Query>())
                .Returns(
                    ImmutableList.Create(
                        new BudgetData(
                            Id: budgetTestData1.Id,
                            Name: "Beverages",
                            SpendingLimit: budgetTestData1.SpendingLimit,
                            CurrentSpending: budgetTestData1.CurrentSpending,
                            MonthlyBudget: budgetTestData1.MonthlyBudget,
                            MonthlySpending: budgetTestData1.CurrentSpending / budgetTestData1.Interval),
                        new BudgetData(
                            Id: budgetTestData1.Id,
                            Name: "Apples",
                            SpendingLimit: budgetTestData1.SpendingLimit,
                            CurrentSpending: budgetTestData1.CurrentSpending,
                            MonthlyBudget: budgetTestData1.MonthlyBudget * 2,
                            MonthlySpending: budgetTestData1.CurrentSpending / budgetTestData1.Interval)));
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
            var expectedAmount = viewModel.Budgets.ToList().Sum(b => b.MonthlyBudget);
            _ = viewModel.BudgetedAmount.Should().Be(expectedAmount);
        }
    }
}
