using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    public class AddCategoryViewModelTests
    {
        [Fact]
        public void Prepare_CategoryCreated()
        {
            // Arrange
            var crudServiceMock = new Mock<ICrudServicesAsync>();

            var addCategoryVm = new AddCategoryViewModel(crudServiceMock.Object, null, null, null, null, null);

            // Act
            addCategoryVm.Prepare(new ModifyCategoryParameter());

            // Assert
            addCategoryVm.SelectedCategory.ShouldNotBeNull();
        }

        [Fact]
        public void Prepare_Title_Set()
        {
            // Arrange
            var crudServiceMock = new Mock<ICrudServicesAsync>();

            var addCategoryVm = new AddCategoryViewModel(crudServiceMock.Object, null, null, null, null, null);

            // Act
            addCategoryVm.Prepare(new ModifyCategoryParameter());

            // Assert
            addCategoryVm.Title.ShouldEqual(Strings.AddCategoryTitle);
        }

    }
}
