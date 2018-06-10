using AutoFixture;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    public class ModifyCategoryGroupViewModelTests
    {
        [Fact]
        public void Title_IsEditFalse_CorrectTitle()
        {
            // Arrange
            var fixture = new Fixture();
            var vm = fixture.Create<ModifyCategoryGroupViewModel>();
            vm.IsEdit = false;

            // Assert
            vm.Title.ShouldEqual(Strings.AddCategoryGroupTitle);
        }
        
        [Fact]
        public void Title_IsEditTrue_CorrectTitle()
        {
            // Arrange
            var fixture = new Fixture();
            var vm = fixture.Create<ModifyCategoryGroupViewModel>();
            vm.IsEdit = true;

            // Assert
            vm.Title.ShouldEqual(Strings.EditCategoryGroupTitle);
        }
    }
}
