using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AddCategoryViewModelTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;

        public AddCategoryViewModelTests()
        {
            mediatorMock = new Mock<IMediator>();
            dialogServiceMock = new Mock<IDialogService>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            backupServiceMock = new Mock<IBackupService>();
        }

        [Fact]
        public void Ctor_Title_Set()
        {
            // Arrange
            // // Act
            var addCategoryVm = new AddCategoryViewModel(mediatorMock.Object, null, null, null, null, null);

            // Assert
            addCategoryVm.Title.ShouldEqual(Strings.AddCategoryTitle);
        }

        [Fact]
        public async Task Initialize_CategoryCreated()
        {
            // Arrange
            var addCategoryVm = new AddCategoryViewModel(mediatorMock.Object, null, null, null, null, null);

            // Act
            await addCategoryVm.InitializeCommand.ExecuteAsync();

            // Assert
            addCategoryVm.SelectedCategory.ShouldNotBeNull();
        }

        [Fact]
        public async Task SaveCategory_EmptyName_ReturnMessage()
        {
            // Arrange
            dialogServiceMock.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(Task.CompletedTask);

            var addCategoryVm = new AddCategoryViewModel(mediatorMock.Object,
                                                         dialogServiceMock.Object,
                                                         settingsFacadeMock.Object,
                                                         backupServiceMock.Object,
                                                         null, null);

            await addCategoryVm.InitializeCommand.ExecuteAsync();

            // Act
            await addCategoryVm.SaveCommand.ExecuteAsync();

            // Assert
            dialogServiceMock.Verify(x => x.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage));
        }
    }
}
