using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    public class EditCategoryViewModelTests
    {
        [Fact]
        public void Prepare_CategoryLoaded()
        {
            // Arrange
            const int categoryId = 99;
            var crudServiceMock = new Mock<ICrudServicesAsync>();
            crudServiceMock.Setup(x => x.ReadSingleAsync<CategoryViewModel>(It.IsAny<int>())).ReturnsAsync(new CategoryViewModel());

            var editAccountVm = new EditCategoryViewModel(crudServiceMock.Object, null, null, null, null, null);

            // Act
            editAccountVm.Prepare(new ModifyCategoryParameter(categoryId));

            // Assert
            crudServiceMock.Verify(x => x.ReadSingleAsync<CategoryViewModel>(categoryId), Times.Once);
        }

        [Fact]
        public void Prepare_Title_Set()
        {
            // Arrange
            const int categoryId = 99;
            var crudServiceMock = new Mock<ICrudServicesAsync>();
            crudServiceMock.Setup(x => x.ReadSingleAsync<CategoryViewModel>(It.IsAny<int>())).ReturnsAsync(new CategoryViewModel());

            var editAccountVm = new EditCategoryViewModel(crudServiceMock.Object, null, null, null, null, null);

            // Act
            editAccountVm.Prepare(new ModifyCategoryParameter(categoryId));

            // Assert
            editAccountVm.Title.ShouldContain(Strings.EditCategoryTitle);
        }
    }
}
