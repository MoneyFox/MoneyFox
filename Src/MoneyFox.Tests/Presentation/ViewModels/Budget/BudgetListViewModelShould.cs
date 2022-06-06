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

    public class BudgetListViewModelShould
    {
        private readonly BudgetListViewModel viewModel;
        private readonly ISender sender;

        protected BudgetListViewModelShould()
        {
            sender = Substitute.For<ISender>();
            viewModel = new BudgetListViewModel(sender);
        }

        private static void AssertBudgetViewModel(BudgetViewModel actualBudgetVm, TestData.IBudget expectedBudgetData)
        {
            actualBudgetVm.Id.Should().Be(expectedBudgetData.Id);
            actualBudgetVm.Name.Should().Be(expectedBudgetData.Name);
            actualBudgetVm.SpendingLimit.Should().Be(expectedBudgetData.SpendingLimit);
            actualBudgetVm.CurrentSpending.Should().Be(expectedBudgetData.CurrentSpending);
        }

        public class WithNoBudgetsAvailable : BudgetListViewModelShould
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

        public class WithBudgetsAvailable : BudgetListViewModelShould
        {
            private readonly TestData.DefaultBudget budgetTestData;

            public WithBudgetsAvailable()
            {
                budgetTestData = new TestData.DefaultBudget();
                sender.Send(Arg.Any<LoadBudgets.Query>())
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
                AssertBudgetViewModel(actualBudgetVm: loadedBudget, expectedBudgetData: budgetTestData);
            }

            [Fact]
            public async Task HasTheRightCount_WhenInitializeIsCalledMultipleTimes()
            {
                // Act
                await viewModel.InitializeCommand.ExecuteAsync(null);
                await viewModel.InitializeCommand.ExecuteAsync(null);

                // Assert
                viewModel.Budgets.Should().HaveCount(1);
                var loadedBudget = viewModel.Budgets.Single();
                AssertBudgetViewModel(actualBudgetVm: loadedBudget, expectedBudgetData: budgetTestData);
            }
        }
    }

}
