using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Interfaces;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AddCategoryViewModelTests
    {
        private readonly Mock<ICrudServicesAsync> crudServiceMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;

        public AddCategoryViewModelTests()
        {
            crudServiceMock = new Mock<ICrudServicesAsync>();
            dialogServiceMock = new Mock<IDialogService>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            backupServiceMock = new Mock<IBackupService>();
        }

        [Fact]
        public void Prepare_CategoryCreated()
        {
            // Arrange
            // Act
            var addCategoryVm = new AddCategoryViewModel(crudServiceMock.Object, null, null, null, null);

            // Assert
            addCategoryVm.SelectedCategory.ShouldNotBeNull();
        }

        [Fact]
        public void Prepare_Title_Set()
        {
            // Arrange
            // // Act
            var addCategoryVm = new AddCategoryViewModel(crudServiceMock.Object, null, null, null, null);

            // Assert
            addCategoryVm.Title.ShouldEqual(Strings.AddCategoryTitle);
        }

        [Fact]
        public void SaveCategory_EmptyName_ReturnMessage()
        {
            // Arrange
            dialogServiceMock.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var addCategoryVm = new AddCategoryViewModel(crudServiceMock.Object,
                dialogServiceMock.Object, 
                settingsFacadeMock.Object,
                backupServiceMock.Object, null);

            // Act
            addCategoryVm.SaveCommand.Execute(null);

            // Assert
            dialogServiceMock.Verify(x => x.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage));
        }
    }
}
