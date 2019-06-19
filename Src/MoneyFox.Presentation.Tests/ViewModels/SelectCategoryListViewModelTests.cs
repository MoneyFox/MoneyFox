using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Presentation.Messages;
using MoneyFox.Presentation.ViewModels;
using Moq;
using MvvmCross.Plugin.Messenger;
using Xunit;
using IDialogService = MoneyFox.ServiceLayer.Interfaces.IDialogService;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class SelectCategoryListViewModelTests
    {
        [Fact]
        public void ItemClick()
        {
            // Arrange
            CategorySelectedMessage passedMessage = null;
            bool closeWasCalled = false;

            var messengerMock = new Mock<IMvxMessenger>();
            messengerMock.Setup(x => x.Publish(It.IsAny<CategorySelectedMessage>()))
                .Callback((CategorySelectedMessage message) => passedMessage = message);

            var navigationMock = new Mock<INavigationService>();
            navigationMock
                .Setup(x => x.GoBack())
                .Callback(() => closeWasCalled = true);

            var viewModel = new SelectCategoryListViewModel(new Mock<ICrudServicesAsync>().Object,
                                                            new Mock<IDialogService>().Object,
                                                            navigationMock.Object);

            var testCategory = new CategoryViewModel();

            // Act
            viewModel.ItemClickCommand.Execute(testCategory);

            // Assert
            Assert.NotNull(passedMessage);
            Assert.Equal(testCategory, passedMessage.SelectedCategory);
            Assert.True(closeWasCalled);
        }

        [Fact]
        public void Close()
        {
            // Arrange
            bool closeWasCalled = false;
            var navigationMock = new Mock<INavigationService>();
            navigationMock
                .Setup(x => x.GoBack())
                .Callback(() => closeWasCalled = true);

            var viewModel = new SelectCategoryListViewModel(new Mock<ICrudServicesAsync>().Object,
                new Mock<IDialogService>().Object,
                navigationMock.Object );

            // Act
            viewModel.ItemClickCommand.Execute(new CategoryViewModel());

            // Assert
            Assert.True(closeWasCalled);
        }
    }
}
