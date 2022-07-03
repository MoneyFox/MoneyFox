namespace MoneyFox.Tests.Presentation.ViewModels.Budget
{

    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MediatR;
    using MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading;
    using MoneyFox.ViewModels.Budget;
    using NSubstitute;
    using TestFramework;
    using Xunit;

    public class BudgetListPageViewModelShould
    {
        private readonly BudgetListViewModel viewModel;
        private readonly ISender sender;

        protected BudgetListPageViewModelShould()
        {
            sender = Substitute.For<ISender>();
            viewModel = new BudgetListViewModel(sender);
        }

        private static void AssertBudgetListViewModel(BudgetListItemViewModel actualBudgetVm, TestData.IBudget expectedBudgetData)
        {
            actualBudgetVm.Id.Should().Be(expectedBudgetData.Id);
            actualBudgetVm.Name.Should().Be(expectedBudgetData.Name);
            actualBudgetVm.SpendingLimit.Should().Be(expectedBudgetData.SpendingLimit);
            actualBudgetVm.CurrentSpending.Should().Be(expectedBudgetData.CurrentSpending);
        }

        public class WithNoBudgetsAvailable : BudgetListPageViewModelShould
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

        public class WithBudgetAvailable : BudgetListPageViewModelShould
        {
            private readonly TestData.DefaultBudget budgetTestData;

            public WithBudgetAvailable()
            {
                budgetTestData = new TestData.DefaultBudget();
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

        public class WithMultipleBudgetAvailable : BudgetListPageViewModelShould
        {
            public WithMultipleBudgetAvailable()
            {
                var budgetTestData1 = new TestData.DefaultBudget();
                sender.Send(Arg.Any<LoadBudgetListData.Query>())
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
                viewModel.Budgets.Should().HaveCount(2);
                viewModel.Budgets[0].Name.Should().Be("Apples");
                viewModel.Budgets[1].Name.Should().Be("Beverages");
            }

            [Fact]
            public async Task TotalAmountCorrectlyCalculated()
            {
                // Act
                await viewModel.InitializeCommand.ExecuteAsync(null);

                // Assert
                var expectedAmount = viewModel.Budgets.Sum(b => b.SpendingLimit);
                viewModel.BudgetedAmount.Should().Be(expectedAmount);
            }
        }
    }

}
