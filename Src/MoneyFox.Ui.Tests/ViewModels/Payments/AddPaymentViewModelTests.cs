namespace MoneyFox.Ui.Tests.ViewModels.Payments;

using AutoMapper;
using Core.Common.Interfaces;
using MediatR;
using NSubstitute;
using Resources.Strings;
using Ui.ViewModels.Payments;
using Xunit;

public sealed class AddPaymentViewModelTests
{
    [Fact]
    public void ShowToast_WhenExceptionThrownDuringSave()
    {
        // Arrange
        var dialogService = Substitute.For<IDialogService>();
        var toastService = Substitute.For<IToastService>();
        var vm = new AddPaymentViewModel(
            mediator: Substitute.For<IMediator>(),
            mapper: Substitute.For<IMapper>(),
            dialogService: dialogService,
            toastService: toastService) { SelectedPayment = new() { ChargedAccount = new() } };

        dialogService.ShowConfirmMessageAsync(title: Arg.Any<string>(), message: Arg.Any<string>()).Returns(true);

        // Act
        vm.SaveCommand.ExecuteAsync(null);

        // Assert
        toastService.Received(1).ShowToastAsync(Translations.UnknownErrorMessage);
    }
}