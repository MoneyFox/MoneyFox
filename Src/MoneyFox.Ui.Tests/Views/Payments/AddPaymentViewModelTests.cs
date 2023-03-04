namespace MoneyFox.Ui.Tests.Views.Payments;

using AutoMapper;
using Core.Common.Interfaces;
using MediatR;
using NSubstitute;
using Ui.Views.Payments.PaymentModification;
using Xunit;

public sealed class AddPaymentViewModelTests
{
    [Fact]
    public void ShowToast_WhenExceptionThrownDuringSave()
    {
        // Arrange
        var dialogService = Substitute.For<IDialogService>();
        var toastService = Substitute.For<IToastService>();
        var mediator = Substitute.For<IMediator>();
        var vm = new AddPaymentViewModel(
            mediator: mediator,
            mapper: Substitute.For<IMapper>(),
            dialogService: dialogService,
            toastService: toastService,
            categorySelectionViewModel: new(mediator: mediator, navigationService: Substitute.For<INavigationService>()))
        {
            SelectedPayment = new() { ChargedAccount = new() }
        };

        dialogService.ShowConfirmMessageAsync(title: Arg.Any<string>(), message: Arg.Any<string>()).Returns(true);

        // Act
        vm.SaveCommand.ExecuteAsync(null);

        // Assert
        toastService.Received(1).ShowToastAsync(Arg.Any<string>());
    }
}
