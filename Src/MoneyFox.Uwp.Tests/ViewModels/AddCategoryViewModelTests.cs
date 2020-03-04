using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels;
using Moq;
using Should;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AddCategoryViewModelTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IDialogService> dialogServiceMock;

        public AddCategoryViewModelTests()
        {
            mediatorMock = new Mock<IMediator>();
            dialogServiceMock = new Mock<IDialogService>();
        }

        [Fact]
        public void Ctor_Title_Set()
        {
            // Arrange
            // // Act
            var addCategoryVm = new AddCategoryViewModel(mediatorMock.Object, null, null, null);

            // Assert
            addCategoryVm.Title.ShouldEqual(Strings.AddCategoryTitle);
        }

        [Fact]
        public async Task Initialize_CategoryCreated()
        {
            // Arrange
            var addCategoryVm = new AddCategoryViewModel(mediatorMock.Object, null, null, null);

            // Act
            await addCategoryVm.InitializeCommand.ExecuteAsync();

            // Assert
            addCategoryVm.SelectedCategory.ShouldNotBeNull();
        }

        [Fact]
        public async Task SaveCategory_EmptyName_ReturnMessage()
        {
            // Arrange
            dialogServiceMock.Setup(x => x.ShowMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(Task.CompletedTask);

            var addCategoryVm = new AddCategoryViewModel(mediatorMock.Object,
                                                         dialogServiceMock.Object,
                                                         null,
                                                         null);

            await addCategoryVm.InitializeCommand.ExecuteAsync();

            // Act
            await addCategoryVm.SaveCommand.ExecuteAsync();

            // Assert
            dialogServiceMock.Verify(x => x.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage));
        }
    }
}
