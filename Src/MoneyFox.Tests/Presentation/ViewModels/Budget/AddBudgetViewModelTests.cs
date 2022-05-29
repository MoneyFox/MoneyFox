namespace MoneyFox.Tests.Presentation.ViewModels.Budget
{

    using FluentAssertions;
    using MediatR;
    using MoneyFox.Core._Pending_.Common.Messages;
    using MoneyFox.ViewModels.Budget;
    using NSubstitute;
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
    }

}
