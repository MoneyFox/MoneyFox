namespace MoneyFox.Ui.Tests.Views.Payments;

using Aptabase.Maui;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Queries;
using Domain;
using Domain.Aggregates.AccountAggregate;
using Domain.Tests.TestFramework;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Ui.Views.Payments.PaymentModification;
using Xunit;

public sealed class EditPaymentViewModelTests
{
    [Fact]
    public async Task SelectedPaymentSet_AfterInitialization()
    {
        // Arrange
        var dialogService = Substitute.For<IDialogService>();
        var toastService = Substitute.For<IToastService>();
        var mediator = Substitute.For<IMediator>();
        var settingsFacade = Substitute.For<ISettingsFacade>();
        settingsFacade.DefaultCurrency.Returns("CHF");
        var dbPayment = new TestData.DefaultExpense().CreateDbPayment();
        var dbAccount = new TestData.DefaultAccount().CreateDbAccount();
        mediator.Send(request: Arg.Any<GetAccountsQuery>(), cancellationToken: Arg.Any<CancellationToken>()).Returns(new List<Account> { dbAccount });
        mediator.Send(request: Arg.Any<GetPaymentByIdQuery>(), cancellationToken: Arg.Any<CancellationToken>()).Returns(dbPayment);
        var vm = new EditPaymentViewModel(
            mediator: mediator,
            dialogService: dialogService,
            toastService: toastService,
            settingsFacade: settingsFacade,
            categorySelectionViewModel: new(navigationService: Substitute.For<INavigationService>()),
            aptabaseClient: Substitute.For<IAptabaseClient>());

        dialogService.ShowConfirmMessageAsync(title: Arg.Any<string>(), message: Arg.Any<string>()).Returns(true);

        // Act
        await vm.InitializeAsync(dbPayment.Id);

        // Assert
        vm.SelectedPayment.Should().NotBeNull();
    }

    [Fact]
    public void ShowToast_WhenExceptionThrownDuringSave()
    {
        // Arrange
        var dialogService = Substitute.For<IDialogService>();
        var toastService = Substitute.For<IToastService>();
        var mediator = Substitute.For<IMediator>();
        var vm = new EditPaymentViewModel(
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
