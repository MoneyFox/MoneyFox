namespace MoneyFox.Ui.Tests.ViewModels.Budget;

using Core.ApplicationCore.UseCases.BudgetCreation;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Interfaces;
using Core.Tests.TestFramework;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Views.Budget;
using Views.Categories;
using Xunit;

public class AddBudgetViewModelTests
{
    private const int CATEGORY_ID = 10;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    private readonly AddBudgetViewModel viewModel;

    public AddBudgetViewModelTests()
    {
        sender = Substitute.For<ISender>();
        navigationService = Substitute.For<INavigationService>();
        dialogService = Substitute.For<IDialogService>();
        viewModel = new(sender: sender, navigationService: navigationService, dialogService: dialogService);
    }

    [Fact]
    public void AddsSelectedCategoryToList()
    {
        // Act
        CategorySelectedMessage categorySelectedMessage = new(new(categoryId: CATEGORY_ID, name: "Beer"));
        viewModel.Receive(categorySelectedMessage);

        // Assert
        _ = viewModel.SelectedCategories.Should().HaveCount(1);
        _ = viewModel.SelectedCategories.Should().Contain(c => c.CategoryId == CATEGORY_ID);
    }

    [Fact]
    public void IgnoresSelectedCategory_WhenEntryWithSameIdAlreadyInList()
    {
        // Act
        CategorySelectedMessage categorySelectedMessage = new(new(categoryId: CATEGORY_ID, name: "Beer"));
        viewModel.Receive(categorySelectedMessage);
        viewModel.Receive(categorySelectedMessage);

        // Assert
        _ = viewModel.SelectedCategories.Should().HaveCount(1);
        _ = viewModel.SelectedCategories.Should().Contain(c => c.CategoryId == CATEGORY_ID);
    }

    [Fact]
    public async Task SendsCorrectSaveCommand()
    {
        // Capture
        CreateBudget.Command? passedQuery = null;
        _ = await sender.Send(Arg.Do<CreateBudget.Command>(q => passedQuery = q));

        // Arrange
        TestData.DefaultBudget testBudget = new();
        viewModel.Name = testBudget.Name;
        viewModel.SpendingLimit = testBudget.SpendingLimit;

        // Act
        viewModel.SelectedCategories.AddRange(testBudget.Categories.Select(c => new BudgetCategoryViewModel(categoryId: c, name: "Category")));
        await viewModel.SaveBudgetCommand.ExecuteAsync(null);

        // Assert
        _ = passedQuery.Should().NotBeNull();
        _ = passedQuery!.Name.Should().Be(testBudget.Name);
        _ = passedQuery.SpendingLimit.Should().Be(testBudget.SpendingLimit);
        _ = passedQuery.Categories.Should().BeEquivalentTo(testBudget.Categories);
        await navigationService.Received(1).GoBackFromModalAsync();
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
        _ = viewModel.SelectedCategories.Should().BeEmpty();
    }

    [Fact]
    public async Task CallNavigationToCategorySelection()
    {
        // Act
        await viewModel.OpenCategorySelectionCommand.ExecuteAsync(null);

        // Assert
        await navigationService.Received(1).OpenModalAsync<SelectCategoryPage>();
    }

    public class SaveShouldBeDisabled : AddBudgetViewModelTests
    {
        [Fact]
        public void OnInitialized()
        {
            // Assert
            _ = viewModel.SaveBudgetCommand.CanExecute(null).Should().BeFalse();
        }

        [Fact]
        public void WhenBudgetNameIsEmpty()
        {
            // Act
            viewModel.Name = string.Empty;

            // Assert
            _ = viewModel.SaveBudgetCommand.CanExecute(null).Should().BeFalse();
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

            // Assert
            _ = viewModel.SaveBudgetCommand.CanExecute(null).Should().BeTrue();
        }
    }
}
