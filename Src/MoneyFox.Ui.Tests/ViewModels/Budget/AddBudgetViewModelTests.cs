namespace MoneyFox.Ui.Tests.ViewModels.Budget;

using Core.ApplicationCore.UseCases.BudgetCreation;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Interfaces;
using Core.Resources;
using Core.Tests.TestFramework;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Views.Budget;
using Views.Categories;
using Xunit;

public class AddBudgetViewModelTests
{
    private const int CategoryId = 10;
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
        CategorySelectedMessage categorySelectedMessage = new(new(categoryId: CategoryId, name: "Beer"));
        viewModel.Receive(categorySelectedMessage);

        // Assert
        viewModel.SelectedCategories.Should().HaveCount(1);
        viewModel.SelectedCategories.Should().Contain(c => c.CategoryId == CategoryId);
    }

    [Fact]
    public void IgnoresSelectedCategory_WhenEntryWithSameIdAlreadyInList()
    {
        // Act
        CategorySelectedMessage categorySelectedMessage = new(new(categoryId: CategoryId, name: "Beer"));
        viewModel.Receive(categorySelectedMessage);
        viewModel.Receive(categorySelectedMessage);

        // Assert
        viewModel.SelectedCategories.Should().HaveCount(1);
        viewModel.SelectedCategories.Should().Contain(c => c.CategoryId == CategoryId);
    }

    [Fact]
    public async Task SendsCorrectSaveCommand()
    {
        // Capture
        CreateBudget.Command? passedQuery = null;
        await sender.Send(Arg.Do<CreateBudget.Command>(q => passedQuery = q));

        // Arrange
        var testBudget = new TestData.DefaultBudget();
        viewModel.SelectedBudget.Name = testBudget.Name;
        viewModel.SelectedBudget.SpendingLimit = testBudget.SpendingLimit;

        // Act
        viewModel.SelectedCategories.AddRange(testBudget.Categories.Select(c => new BudgetCategoryViewModel(categoryId: c, name: "Category")));
        await viewModel.SaveBudgetCommand.ExecuteAsync(null);

        // Assert
        _ = passedQuery.Should().NotBeNull();
        passedQuery!.Name.Should().Be(testBudget.Name);
        passedQuery.SpendingLimit.Should().Be(testBudget.SpendingLimit);
        passedQuery.Categories.Should().BeEquivalentTo(testBudget.Categories);
        await navigationService.Received(1).GoBackFromModalAsync();
    }

    [Fact]
    public void Removes_SelectedCategory_OnCommand()
    {
        // Arrange
        var budgetCategoryViewModel = new BudgetCategoryViewModel(categoryId: 1, name: "test");
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
        await navigationService.Received(1).OpenModalAsync<SelectCategoryPage>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task ShowMessage_WhenSpendingLimitIsInvalid(decimal amount)
    {
        // Arrange
        var testBudget = new TestData.DefaultBudget();
        viewModel.SelectedBudget.Name = testBudget.Name;
        viewModel.SelectedBudget.SpendingLimit = amount;

        // Act
        await viewModel.SaveBudgetCommand.ExecuteAsync(null);

        // Assert
        await dialogService.Received().ShowMessageAsync(title: Strings.InvalidSpendingLimitTitle, message: Strings.InvalidSpendingLimitMessage);
        await sender.Received(0).Send(Arg.Any<CreateBudget.Command>());
        await navigationService.Received(0).GoBackFromModalAsync();
    }
}
