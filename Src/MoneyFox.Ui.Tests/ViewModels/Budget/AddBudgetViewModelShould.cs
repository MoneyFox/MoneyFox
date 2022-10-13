namespace MoneyFox.Ui.Tests.ViewModels.Budget;

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using MoneyFox.Core.ApplicationCore.UseCases.BudgetCreation;
using MoneyFox.Core.Common.Extensions;
using MoneyFox.Core.Common.Messages;
using MoneyFox.Core.Interfaces;
using MoneyFox.Tests.TestFramework;
using MoneyFox.Ui.ViewModels.Budget;
using NSubstitute;
using Views.Categories;
using Xunit;

public class AddBudgetViewModelShould
{
    private const int CategoryId = 10;
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    private readonly AddBudgetViewModel viewModel;

    public AddBudgetViewModelShould()
    {
        sender = Substitute.For<ISender>();
        navigationService = Substitute.For<INavigationService>();
        viewModel = new AddBudgetViewModel(sender: sender, navigationService: navigationService);
    }

    [Fact]
    public void AddsSelectedCategoryToList()
    {
        // Act
        CategorySelectedMessage categorySelectedMessage = new(new CategorySelectedDataSet(categoryId: CategoryId, name: "Beer"));
        viewModel.Receive(categorySelectedMessage);

        // Assert
        viewModel.SelectedCategories.Should().HaveCount(1);
        viewModel.SelectedCategories.Should().Contain(c => c.CategoryId == CategoryId);
    }

    [Fact]
    public void IgnoresSelectedCategory_WhenEntryWithSameIdAlreadyInList()
    {
        // Act
        CategorySelectedMessage categorySelectedMessage = new(new CategorySelectedDataSet(categoryId: CategoryId, name: "Beer"));
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
        _ = await sender.Send(Arg.Do<CreateBudget.Command>(q => passedQuery = q));

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
}

