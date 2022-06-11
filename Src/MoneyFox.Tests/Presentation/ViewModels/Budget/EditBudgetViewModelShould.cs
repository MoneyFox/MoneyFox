namespace MoneyFox.Tests.Presentation.ViewModels.Budget
{

    using System.Threading.Tasks;
    using FluentAssertions;
    using MediatR;
    using MoneyFox.Core.Common.Messages;
    using MoneyFox.Core.Interfaces;
    using MoneyFox.ViewModels.Budget;
    using NSubstitute;
    using Views.Categories;
    using Xunit;

    public class EditBudgetViewModelShould
    {
        private const int CATEGORY_ID = 10;
        private readonly ISender sender;

        private readonly EditBudgetViewModel viewModel;
        private readonly INavigationService navigationService;

        public EditBudgetViewModelShould()
        {
            sender = Substitute.For<ISender>();
            navigationService = Substitute.For<INavigationService>();
            viewModel = new EditBudgetViewModel(sender: sender, navigationService: navigationService);
        }

        [Fact]
        public void AddsSelectedCategoryToList()
        {
            // Act
            var categorySelectedMessage = new CategorySelectedMessage(new CategorySelectedDataSet(categoryId: CATEGORY_ID, name: "Beer"));
            viewModel.Receive(categorySelectedMessage);

            // Assert
            viewModel.SelectedCategories.Should().HaveCount(1);
            viewModel.SelectedCategories.Should().Contain(c => c.CategoryId == CATEGORY_ID);
        }

        [Fact]
        public void IgnoresSelectedCategory_WhenEntryWithSameIdAlreadyInList()
        {
            // Act
            var categorySelectedMessage = new CategorySelectedMessage(new CategorySelectedDataSet(categoryId: CATEGORY_ID, name: "Beer"));
            viewModel.Receive(categorySelectedMessage);
            viewModel.Receive(categorySelectedMessage);

            // Assert
            viewModel.SelectedCategories.Should().HaveCount(1);
            viewModel.SelectedCategories.Should().Contain(c => c.CategoryId == CATEGORY_ID);
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
            await navigationService.Received(1).OpenModal<SelectCategoryPage>();
        }

        [Fact]
        public async Task SendsCorrectSaveCommand()
        {

        }
    }

}
