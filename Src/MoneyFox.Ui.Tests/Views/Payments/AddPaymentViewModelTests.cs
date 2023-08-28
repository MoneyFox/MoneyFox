namespace MoneyFox.Ui.Tests.Views.Payments;

using Aptabase.Maui;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Domain;
using MediatR;
using Ui.Views.Payments.PaymentModification;

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
            dialogService: dialogService,
            toastService: toastService,
            settingsFacade: Substitute.For<ISettingsFacade>(),
            categorySelectionViewModel: new(navigationService: Substitute.For<INavigationService>()),
            aptabaseClient: Substitute.For<IAptabaseClient>())
        {
            SelectedPayment = new() { ChargedAccount = new(Id: 1, Name: "", CurrentBalance: Money.Zero("CHF")) }
        };

        dialogService.ShowConfirmMessageAsync(title: Arg.Any<string>(), message: Arg.Any<string>()).Returns(true);

        // Act
        vm.SaveCommand.ExecuteAsync(null);

        // Assert
        toastService.Received(1).ShowToastAsync(Arg.Any<string>());
    }
}
