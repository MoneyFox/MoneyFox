using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MediatR;
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

            var viewModel = new SelectCategoryListViewModel(new Mock<IMediator>().Object,
                                                            new Mock<IMapper>().Object,
                                                            new Mock<IDialogService>().Object,
                                                            navigationMock.Object,
                                                            new Mock<IMessenger>().Object);

            // Act
            viewModel.ItemClickCommand.Execute(new CategoryViewModel());

            // Assert
            navigationMock.Verify(x => x.GoBack(), Times.Once);
        }
    }
}
