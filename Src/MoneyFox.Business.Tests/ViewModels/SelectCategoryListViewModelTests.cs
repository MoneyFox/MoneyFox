using System.Threading.Tasks;
using MoneyFox.Business.Messages;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Interfaces;
using Moq;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
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
                .Setup(x => x.Close(It.IsAny<IMvxViewModel>()))
                .Callback((IMvxViewModel vm) => closeWasCalled = true)
                .Returns(Task.FromResult(true));

            var viewModel = new SelectCategoryListViewModel(new Mock<ICategoryService>().Object,
                new Mock<IModifyDialogService>().Object,
                new Mock<IDialogService>().Object,
                messengerMock.Object,
                navigationMock.Object );

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
                .Setup(x => x.Close(It.IsAny<IMvxViewModel>()))
                .Callback((IMvxViewModel vm) => closeWasCalled = true)
                .Returns(Task.FromResult(true));

            var viewModel = new SelectCategoryListViewModel(new Mock<ICategoryService>().Object,
                new Mock<IModifyDialogService>().Object,
                new Mock<IDialogService>().Object,
                new Mock<IMvxMessenger>().Object,
                navigationMock.Object );

            // Act
            await viewModel.ItemClickCommand.ExecuteAsync(new CategoryViewModel(new Category()));

            // Assert
            Assert.True(closeWasCalled);
        }
    }
}
