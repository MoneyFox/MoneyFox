using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Xunit;
using IDialogService = MoneyFox.ServiceLayer.Interfaces.IDialogService;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class SelectCategoryListViewModelTests
    {
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
