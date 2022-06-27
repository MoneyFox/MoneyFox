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
        private readonly BudgetListPageViewModel pageViewModel;
        private readonly ISender sender;

        protected BudgetListPageViewModelShould()
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

        public class WithNoBudgetsAvailable : BudgetListPageViewModelShould
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
                await pageViewModel.InitializeCommand.ExecuteAsync(null);

                // Assert
                pageViewModel.Budgets.Should().HaveCount(2);
                pageViewModel.Budgets[0].Name.Should().Be("Apples");
                pageViewModel.Budgets[1].Name.Should().Be("Beverages");
            }

            [Fact]
            public async Task TotalAmountCorrectlyCalculated()
            {
                // Act
                await pageViewModel.InitializeCommand.ExecuteAsync(null);

                // Assert
                var expectedAmount = pageViewModel.Budgets.Sum(b => b.SpendingLimit);
                pageViewModel.BudgetedAmount.Should().Be(expectedAmount);
            }
        }
    }

}
