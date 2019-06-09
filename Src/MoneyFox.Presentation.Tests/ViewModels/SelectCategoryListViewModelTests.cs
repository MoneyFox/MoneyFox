using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Messages;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class SelectCategoryListViewModelTests
    {
        [Fact]
        public async void ItemClick()
        {
            // Arrange
            CategorySelectedMessage passedMessage = null;
            bool closeWasCalled = false;

            var messengerMock = new Mock<IMvxMessenger>();
            messengerMock.Setup(x => x.Publish(It.IsAny<CategorySelectedMessage>()))
                .Callback((CategorySelectedMessage message) => passedMessage = message);

            var navigationMock = new Mock<IMvxNavigationService>();
            navigationMock
                .Setup(x => x.Close(It.IsAny<IMvxViewModel>(), CancellationToken.None))
                .Callback((IMvxViewModel vm, CancellationToken t) => closeWasCalled = true)
                .Returns(Task.FromResult(true));

            var viewModel = new SelectCategoryListViewModel(new Mock<ICrudServicesAsync>().Object,
                                                            new Mock<IDialogService>().Object,
                                                            messengerMock.Object,
                                                            new Mock<IMvxLogProvider>().Object,
                                                            navigationMock.Object);

            var testCategory = new CategoryViewModel();

            // Act
            await viewModel.ItemClickCommand.ExecuteAsync(testCategory);

            // Assert
            Assert.NotNull(passedMessage);
            Assert.Equal(testCategory, passedMessage.SelectedCategory);
            Assert.True(closeWasCalled);
        }

        [Fact]
        public async void Close()
        {
            // Arrange
            bool closeWasCalled = false;
            var navigationMock = new Mock<IMvxNavigationService>();
            navigationMock
                .Setup(x => x.Close(It.IsAny<IMvxViewModel>(), CancellationToken.None))
                .Callback((IMvxViewModel vm, CancellationToken t) => closeWasCalled = true)
                .Returns(Task.FromResult(true));

            var viewModel = new SelectCategoryListViewModel(new Mock<ICrudServicesAsync>().Object,
                new Mock<IDialogService>().Object,
                new Mock<IMvxMessenger>().Object,
                new Mock<IMvxLogProvider>().Object,
                navigationMock.Object );

            // Act
            await viewModel.ItemClickCommand.ExecuteAsync(new CategoryViewModel());

            // Assert
            Assert.True(closeWasCalled);
        }
    }
}
