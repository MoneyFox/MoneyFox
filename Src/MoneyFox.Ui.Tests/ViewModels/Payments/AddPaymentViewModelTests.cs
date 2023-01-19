namespace MoneyFox.Ui.Tests.ViewModels.Payments;

using AutoMapper;
using MediatR;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Ui.Resources.Strings;
using MoneyFox.Ui.ViewModels.Payments;
using NSubstitute;
using Xunit;

public sealed class AddPaymentViewModelTests
{
    [Fact]
    public void ShowToast_WhenExceptionThrownDuringSave()
    {
        // Arrange
        IDialogService dialogService = Substitute.For<IDialogService>();
        IToastService toastService = Substitute.For<IToastService>();

        var vm = new AddPaymentViewModel(Substitute.For<IMediator>(),
                                     Substitute.For<IMapper>(),
                                     dialogService,
                                     toastService);

        dialogService.ShowConfirmMessageAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        // Act
        vm.SaveCommand.ExecuteAsync(null);

        // Assert
        toastService.Received(1).ShowToastAsync(Translations.UnknownErrorMessage);
    }
}
