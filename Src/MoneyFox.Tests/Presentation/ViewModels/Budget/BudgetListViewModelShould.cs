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
    using TestFramework.Budget;
    using Xunit;

    public sealed class BudgetListViewModelShould
    {
        private readonly BudgetListViewModel viewModel;
        private readonly ISender sender;

        public BudgetListViewModelShould()
        {
            sender = Substitute.For<ISender>();
            viewModel = new BudgetListViewModel(sender);
        }

        [Fact]
        public async Task InitializeBudgetsCollectionEmpty_WhenNotItemsFound()
        {
            // Act
            await viewModel.InitializeCommand.ExecuteAsync(null);

            // Assert
            viewModel.Budgets.Should().BeEmpty();
        }

        [Fact]
        public async Task InitializeBudgetsCollection_WithLoadedBudgets()
        {
            // Arrange
            var budgetTestData = new TestData.DefaultBudget();
            sender.Send(Arg.Any<LoadBudgets.Query>())
                .Returns(ImmutableList.Create(new BudgetListData(name: budgetTestData.Name, spendingLimit: budgetTestData.SpendingLimit)));

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
            // Arrange
            var budgetTestData = new TestData.DefaultBudget();
            sender.Send(Arg.Any<LoadBudgets.Query>())
                .Returns(ImmutableList.Create(new BudgetListData(name: budgetTestData.Name, spendingLimit: budgetTestData.SpendingLimit)));

            // Act
            await viewModel.InitializeCommand.ExecuteAsync(null);
            await viewModel.InitializeCommand.ExecuteAsync(null);

            // Assert
            viewModel.Budgets.Should().HaveCount(1);
            var loadedBudget = viewModel.Budgets.Single();
            AssertBudgetViewModel(actualBudgetVm: loadedBudget, expectedBudgetData: budgetTestData);
        }

        private static void AssertBudgetViewModel(BudgetViewModel actualBudgetVm, TestData.IBudget expectedBudgetData)
        {
            actualBudgetVm.Name.Should().Be(expectedBudgetData.Name);
            actualBudgetVm.SpendingLimit.Should().Be(expectedBudgetData.SpendingLimit);
        }
    }

}
