using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Parameters;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.ViewModels;
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
            var addCategoryVm = new AddCategoryViewModel(crudServiceMock.Object, null, null, null, null, null);

            // Act
            addCategoryVm.Prepare(new ModifyCategoryParameter());

            // Assert
            addCategoryVm.Title.ShouldEqual(Strings.AddCategoryTitle);
        }

        [Fact]
        public async Task SaveCategory_EmptyName_ReturnMessage()
        {
            // Arrange
            dialogServiceMock.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var addCategoryVm = new AddCategoryViewModel(crudServiceMock.Object,
                dialogServiceMock.Object, 
                settingsFacadeMock.Object,
                backupServiceMock.Object, null, null);
            addCategoryVm.Prepare(new ModifyCategoryParameter());

            // Act
            await addCategoryVm.SaveCommand.ExecuteAsync();

            // Assert
            dialogServiceMock.Verify(x => x.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage));
        }
    }
}
