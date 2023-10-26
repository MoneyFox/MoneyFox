namespace MoneyFox.Ui.Tests.Views.Budget;

using System.Collections.Immutable;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Features.BudgetDeletion;
using Core.Features.BudgetUpdate;
using Core.Queries;
using Core.Queries.BudgetEntryLoading;
using Domain.Aggregates.CategoryAggregate;
using Domain.Tests.TestFramework;
using MediatR;
using Ui.Views.Budget.BudgetModification;
using Ui.Views.Categories.CategorySelection;

public class EditBudgetViewModelTests
{
    private const int CATEGORY_ID = 10;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    private readonly EditBudgetViewModel viewModel;

    protected EditBudgetViewModelTests()
    {
        sender = Substitute.For<ISender>();
        navigationService = Substitute.For<INavigationService>();
        dialogService = Substitute.For<IDialogService>();
        viewModel = new(sender: sender, navigationService: navigationService, dialogService: dialogService);
    }

    public class OnReceiveMessage : EditBudgetViewModelTests
    {
        public OnReceiveMessage()
        {
            sender.Send(Arg.Any<GetCategoryByIdQuery>()).Returns(new Category("Beer"));
        }

        [Fact]
        public void AddsSelectedCategoryToList()
        {
            // Act
            viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { SelectCategoryViewModel.SELECTED_CATEGORY_ID_PARAM, CATEGORY_ID } });

            // Assert
            _ = viewModel.SelectedCategories.Should().HaveCount(1);
            _ = viewModel.SelectedCategories.Should().Contain(c => c.CategoryId == CATEGORY_ID);
        }

        [Fact]
        public void IgnoresSelectedCategory_WhenEntryWithSameIdAlreadyInList()
        {
            // Act
            viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { SelectCategoryViewModel.SELECTED_CATEGORY_ID_PARAM, CATEGORY_ID } });
            viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { SelectCategoryViewModel.SELECTED_CATEGORY_ID_PARAM, CATEGORY_ID } });

            // Assert
            _ = viewModel.SelectedCategories.Should().HaveCount(1);
            _ = viewModel.SelectedCategories.Should().Contain(c => c.CategoryId == CATEGORY_ID);
        }
    }

    public class OnRemoveCategory : EditBudgetViewModelTests
    {
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
    }

    public class OnOpenCategorySelection : EditBudgetViewModelTests
    {
        [Fact]
        public async Task CallNavigationToCategorySelection()
        {
            // Act
            await viewModel.OpenCategorySelectionCommand.ExecuteAsync(null);

            // Assert
            await navigationService.Received(1).OpenModalAsync<SelectCategoryPage>();
        }
    }

    public class OnSaveBudget : EditBudgetViewModelTests
    {
        [Fact]
        public async Task SendsCorrectSaveCommand()
        {
            // Capture
            UpdateBudget.Command? capturedCommand = null;
            await sender.Send(Arg.Do<UpdateBudget.Command>(q => capturedCommand = q));

            // Arrange
            TestData.DefaultBudget testBudget = new();
            viewModel.Name = testBudget.Name;
            viewModel.SpendingLimit = testBudget.SpendingLimit;

            // Act
            viewModel.SelectedCategories.AddRange(
                testBudget.Categories.Select<int, BudgetCategoryViewModel>(selector: c => new(categoryId: c, name: "Category")));

            await viewModel.SaveBudgetCommand.ExecuteAsync(null);

            // Assert
            _ = capturedCommand.Should().NotBeNull();
            _ = capturedCommand!.Name.Should().Be(testBudget.Name);
            _ = capturedCommand.SpendingLimit.Should().Be(testBudget.SpendingLimit);
            _ = capturedCommand.Categories.Should().BeEquivalentTo(testBudget.Categories);
            await navigationService.Received(1).GoBackFromModalAsync();
        }
    }

    public class OnInitialize : EditBudgetViewModelTests
    {
        [Fact]
        public async Task SendCorrectLoadingCommand()
        {
            // Capture
            TestData.DefaultBudget testBudget = new();
            var categories = testBudget.Categories.Select<int, BudgetEntryData.BudgetCategory>(selector: c => new(id: c, name: "category")).ToImmutableList();
            LoadBudgetEntry.Query? capturedQuery = null;
            _ = sender.Send(Arg.Do<LoadBudgetEntry.Query>(q => capturedQuery = q))
                .Returns(
                    new BudgetEntryData(
                        id: new(testBudget.Id),
                        name: testBudget.Name,
                        spendingLimit: testBudget.SpendingLimit,
                        numberOfMonths: testBudget.Interval.NumberOfMonths,
                        categories: categories));

            // Arrange

            // Act
            await viewModel.InitializeCommand.ExecuteAsync(testBudget.Id);

            // Assert
            _ = capturedQuery.Should().NotBeNull();
            _ = capturedQuery!.BudgetId.Should().Be(testBudget.Id);
            _ = viewModel.Id.Value.Should().Be(testBudget.Id);
            _ = viewModel.Name.Should().Be(testBudget.Name);
            _ = viewModel.SpendingLimit.Should().Be(testBudget.SpendingLimit);
            _ = viewModel.NumberOfMonths.Should().Be(testBudget.Interval.NumberOfMonths);
            _ = viewModel.SelectedCategories[0].CategoryId.Should().Be(categories[0].Id);
            _ = viewModel.SelectedCategories[0].Name.Should().Be(categories[0].Name);
        }
    }

    public class OnDelete : EditBudgetViewModelTests
    {
        [Fact]
        public async Task SendsCorrectDeleteCommand()
        {
            // Capture
            DeleteBudget.Command? capturedCommand = null;
            await sender.Send(Arg.Do<DeleteBudget.Command>(q => capturedCommand = q));

            // Arrange
            _ = dialogService.ShowConfirmMessageAsync(title: Arg.Any<string>(), message: Arg.Any<string>(), positiveButtonText: Arg.Any<string>())
                .Returns(true);

            var testBudget = new TestData.DefaultBudget();
            _ = sender.Send(Arg.Any<LoadBudgetEntry.Query>())
                .Returns(
                    new BudgetEntryData(
                        id: new(testBudget.Id),
                        name: testBudget.Name,
                        spendingLimit: testBudget.SpendingLimit,
                        numberOfMonths: testBudget.Interval.NumberOfMonths,
                        categories: new List<BudgetEntryData.BudgetCategory>()));

            await viewModel.InitializeCommand.ExecuteAsync(testBudget.Id);

            // Act
            await viewModel.DeleteBudgetCommand.ExecuteAsync(null);

            // Assert
            _ = capturedCommand.Should().NotBeNull();
            _ = capturedCommand!.BudgetId.Value.Should().Be(testBudget.Id);
            await navigationService.Received(1).GoBackFromModalAsync();
        }

        [Fact]
        public async Task DoNotSendCommand_WhenConfirmationWasDenied()
        {
            // Arrange
            _ = dialogService.ShowConfirmMessageAsync(title: Arg.Any<string>(), message: Arg.Any<string>()).Returns(false);

            // Act
            await viewModel.DeleteBudgetCommand.ExecuteAsync(null);

            // Assert
            await sender.Received(0).Send(Arg.Any<DeleteBudget.Command>());
            await navigationService.Received(0).GoBackFromModalAsync();
        }
    }

    public class SaveShouldBeDisabled : EditBudgetViewModelTests
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
            viewModel.SpendingLimit = 10;
            viewModel.NumberOfMonths = 1;

            // Assert
            _ = viewModel.SaveBudgetCommand.CanExecute(null).Should().BeFalse();
        }

        [Fact]
        public void WhenNumberOfMonthsIsZero()
        {
            // Act
            viewModel.SpendingLimit = 10;
            viewModel.SpendingLimit = 0;
            viewModel.NumberOfMonths = 1;

            // Assert
            _ = viewModel.SaveBudgetCommand.CanExecute(null).Should().BeFalse();
        }
    }

    public class SaveShouldBeEnabled : EditBudgetViewModelTests
    {
        [Theory]
        [InlineData(" ")]
        [InlineData("Test")]
        public void SaveShouldBeEnabled_WhenBudgetNameIsNotEmptyAndSpendingLimitNotZero(string budgetName)
        {
            // Act
            viewModel.Name = budgetName;
            viewModel.SpendingLimit = 10;
            viewModel.NumberOfMonths = 1;

            // Assert
            _ = viewModel.SaveBudgetCommand.CanExecute(null).Should().BeTrue();
        }
    }
}
