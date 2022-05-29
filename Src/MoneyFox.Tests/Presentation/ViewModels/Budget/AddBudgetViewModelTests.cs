namespace MoneyFox.Tests.Presentation.ViewModels.Budget
{

    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MediatR;
    using MoneyFox.Core._Pending_.Common.Extensions;
    using MoneyFox.Core._Pending_.Common.Messages;
    using MoneyFox.Core.ApplicationCore.UseCases.BudgetCreation;
    using MoneyFox.ViewModels.Budget;
    using NSubstitute;
    using TestFramework.Budget;
    using Xunit;

    public class AddBudgetViewModelTests
    {
        private const int CATEGORY_ID = 10;
        private readonly ISender sender;

        public AddBudgetViewModelTests()
        {
            sender = Substitute.For<ISender>();
        }

        [Fact]
        public void AddsSelectedCategoryToList()
        {
            // Arrange
            var vm = new AddBudgetViewModel(sender);

            // Act
            var categorySelectedMessage = new CategorySelectedMessage(new CategorySelectedDataSet(categoryId: CATEGORY_ID, name: "Beer"));
            vm.Receive(categorySelectedMessage);

            // Assert
            vm.SelectedCategories.Should().HaveCount(1);
            vm.SelectedCategories.Should().Contain(c => c.CategoryId == CATEGORY_ID);
        }

        [Fact]
        public void IgnoresSelectedCategory_WhenEntryWithSameIdAlreadyInList()
        {
            // Arrange
            var vm = new AddBudgetViewModel(sender);

            // Act
            var categorySelectedMessage = new CategorySelectedMessage(new CategorySelectedDataSet(categoryId: CATEGORY_ID, name: "Beer"));
            vm.Receive(categorySelectedMessage);
            vm.Receive(categorySelectedMessage);

            // Assert
            vm.SelectedCategories.Should().HaveCount(1);
            vm.SelectedCategories.Should().Contain(c => c.CategoryId == CATEGORY_ID);
        }

        [Fact]
        public async Task SendsCorrectSaveCommand()
        {
            // Capture
            CreateBudget.Query? passedQuery = null;
            await sender.Send(Arg.Do<CreateBudget.Query>(q => passedQuery = q));

            // Arrange
            var testBudget = new TestData.DefaultBudget();

            // Act
            var vm = new AddBudgetViewModel(sender) { SelectedBudget = { Name = testBudget.Name, SpendingLimit = testBudget.SpendingLimit } };
            vm.SelectedCategories.AddRange(testBudget.Categories.Select(c => new BudgetCategoryViewModel(c, "Category")));
            await vm.SaveBudgetCommand.ExecuteAsync(null);

            // Assert
            passedQuery.Should().NotBeNull();
            passedQuery!.Name.Should().Be(testBudget.Name);
            passedQuery.SpendingLimit.Should().Be(testBudget.SpendingLimit);
            passedQuery.Categories.Should().BeEquivalentTo(testBudget.Categories);
        }
    }

}
