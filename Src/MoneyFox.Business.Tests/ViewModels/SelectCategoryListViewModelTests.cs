using System.Threading;
using System.Threading.Tasks;
using MoneyFox.Business.Messages;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Interfaces;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Tests;
using MvvmCross.ViewModels;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    public class SelectCategoryListViewModelTests : MvxIoCSupportingTest
    {
        [Fact]
        public async void ItemClick()
        {
            // Arrange
            base.Setup();
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

            var viewModel = new SelectCategoryListViewModel(new Mock<ICategoryService>().Object,
                                                            new Mock<IDialogService>().Object,
                                                            messengerMock.Object,
                                                            new Mock<IMvxLogProvider>().Object,
                                                            navigationMock.Object);

            var testCategory = new CategoryViewModel(new Category());

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

            var viewModel = new SelectCategoryListViewModel(new Mock<ICategoryService>().Object,
                new Mock<IDialogService>().Object,
                new Mock<IMvxMessenger>().Object,
                new Mock<IMvxLogProvider>().Object,
                navigationMock.Object );

            // Act
            await viewModel.ItemClickCommand.ExecuteAsync(new CategoryViewModel(new Category()));

            // Assert
            Assert.True(closeWasCalled);
        }
    }
}
