namespace MoneyFox.Ui.Tests.Views.Budget;

using Common.Navigation;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Features.BudgetCreation;
using Core.Queries;
using Domain.Aggregates.CategoryAggregate;
using Domain.Tests.TestFramework;
using MediatR;
using Ui.Views.Budget.BudgetModification;
using Ui.Views.Categories.CategorySelection;

public class AddBudgetViewModelTests
{
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    private readonly AddBudgetViewModel viewModel;

    public AddBudgetViewModelTests()
    {
        sender = Substitute.For<ISender>();
        sender.Send(Arg.Any<GetCategoryByIdQuery>()).Returns(new Category("Beer"));
        navigationService = Substitute.For<INavigationService>();
        var dialogService = Substitute.For<IDialogService>();
        Substitute.For<IDialogService>();
        viewModel = new(sender: sender, navigationService: navigationService, dialogService: dialogService);
    }

    [Fact]
    public async Task SendsCorrectSaveCommand()
    {
        // Capture
        CreateBudget.Command? passedQuery = null;
        await sender.Send(Arg.Do<CreateBudget.Command>(q => passedQuery = q));

        // Arrange
        TestData.DefaultBudget testBudget = new();
        viewModel.Name = testBudget.Name;
        viewModel.SpendingLimit = testBudget.SpendingLimit;
        viewModel.NumberOfMonths = 2;

        // Act
        viewModel.SelectedCategories.AddRange(testBudget.Categories.Select<int, BudgetCategoryViewModel>(selector: c => new(categoryId: c, name: "Category")));
        await viewModel.SaveBudgetCommand.ExecuteAsync(null);

        // Assert
        passedQuery.Should().NotBeNull();
        passedQuery!.Name.Should().Be(testBudget.Name);
        passedQuery.SpendingLimit.Should().Be(testBudget.SpendingLimit);
        passedQuery.Categories.Should().BeEquivalentTo(testBudget.Categories);
        await navigationService.Received(1).GoBack();
    }

    [Fact]
    public void Removes_SelectedCategory_OnCommand()
    {
        // Arrange
        BudgetCategoryViewModel budgetCategoryViewModel = new(categoryId: 1, name: "test");
        viewModel.SelectedCategories.Add(budgetCategoryViewModel);

        // Act
        viewModel.RemoveCategoryCommand.Execute(budgetCategoryViewModel);

        // Assert
        viewModel.SelectedCategories.Should().BeEmpty();
    }

    [Fact]
    public async Task CallNavigationToCategorySelection()
    {
        // Act
        await viewModel.OpenCategorySelectionCommand.ExecuteAsync(null);

        // Assert
        await navigationService.Received(1).GoTo<SelectCategoryViewModel>();
    }

    public class SaveShouldBeDisabled : AddBudgetViewModelTests
    {
        [Fact]
        public void OnInitialized()
        {
            // Assert
            viewModel.SaveBudgetCommand.CanExecute(null).Should().BeFalse();
        }

        [Fact]
        public void WhenBudgetNameIsEmpty()
        {
            // Act
            viewModel.Name = string.Empty;

            // Assert
            viewModel.SaveBudgetCommand.CanExecute(null).Should().BeFalse();
        }
    }

    public class SaveShouldBeEnabled : AddBudgetViewModelTests
    {
        [Theory]
        [InlineData(" ")]
        [InlineData("Test")]
        public void SaveShouldBeEnabled_WhenBudgetNameIsNotEmptyAndSpendingLimitIsNotZero(string budgetName)
        {
            // Act
            viewModel.Name = budgetName;
            viewModel.SpendingLimit = 10;
            viewModel.NumberOfMonths = 2;

            // Assert
            viewModel.SaveBudgetCommand.CanExecute(null).Should().BeTrue();
        }
    }
}
