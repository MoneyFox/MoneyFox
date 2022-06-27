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
        private readonly BudgetListPageViewModel pageViewModel;
        private readonly ISender sender;

        protected BudgetListViewModelShould()
        {
            sender = Substitute.For<ISender>();
            pageViewModel = new BudgetListPageViewModel(sender);
        }

        private static void AssertBudgetListViewModel(BudgetListViewModel actualBudgetVm, TestData.IBudget expectedBudgetData)
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
                await pageViewModel.InitializeCommand.ExecuteAsync(null);

                // Assert
                pageViewModel.Budgets.Should().BeEmpty();
            }
        }

        public class WithBudgetsAvailable : BudgetListViewModelShould
        {
            private readonly TestData.DefaultBudget budgetTestData;

            public WithBudgetsAvailable()
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
                await pageViewModel.InitializeCommand.ExecuteAsync(null);

                // Assert
                pageViewModel.Budgets.Should().HaveCount(1);
                var loadedBudget = pageViewModel.Budgets.Single();
                AssertBudgetListViewModel(actualBudgetVm: loadedBudget, expectedBudgetData: budgetTestData);
            }

            [Fact]
            public async Task DoesNotAddEntriesTwice_WhenInitializeIsCalledMultipleTimes()
            {
                // Act
                await pageViewModel.InitializeCommand.ExecuteAsync(null);
                await pageViewModel.InitializeCommand.ExecuteAsync(null);

                // Assert
                pageViewModel.Budgets.Should().HaveCount(1);
                var loadedBudget = pageViewModel.Budgets.Single();
                AssertBudgetListViewModel(actualBudgetVm: loadedBudget, expectedBudgetData: budgetTestData);
            }
        }
    }

}
