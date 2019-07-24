using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Xunit;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class SelectCategoryListViewModelTests
    {
        [Fact]
        public void Close()
        {
            // Arrange
            var navigationMock = new Mock<INavigationService>();
            navigationMock.Setup(x => x.GoBack());

            var viewModel = new SelectCategoryListViewModel(new Mock<ICrudServicesAsync>().Object,
                new Mock<IDialogService>().Object,
                navigationMock.Object);

            // Act
            viewModel.ItemClickCommand.Execute(new CategoryViewModel());

            // Assert
            navigationMock.Verify(x => x.GoBack(), Times.Once);
        }
    }
}
