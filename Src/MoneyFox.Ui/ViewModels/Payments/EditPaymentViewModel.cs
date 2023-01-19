namespace MoneyFox.Ui.ViewModels.Payments;

using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.ApplicationCore.Queries;
using Core.Commands.Payments.DeletePaymentById;
using Core.Commands.Payments.UpdatePayment;
using Core.Common.Interfaces;
using MediatR;
using Resources.Strings;

internal sealed class EditPaymentViewModel : ModifyPaymentViewModel
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly IDialogService dialogService;
    private readonly IToastService toastService;

    public EditPaymentViewModel(IMediator mediator,
                                IMapper mapper,
                                IDialogService dialogService,
                                IToastService toastService) : base(
        mediator: mediator,
        mapper: mapper,
        dialogService: dialogService,
        toastService: toastService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
        this.toastService = toastService;
    }

    public AsyncRelayCommand<PaymentViewModel> DeleteCommand => new(async p => await DeletePaymentAsync(p));

    public async Task InitializeAsync(int paymentId)
    {
        if (IsFirstLoad is false)
        {
            return;
        }

        await InitializeAsync();
        var payment = await mediator.Send(new GetPaymentByIdQuery(paymentId));
        SelectedPayment = mapper.Map<PaymentViewModel>(payment);
        IsFirstLoad = false;
    }

    protected override async Task SavePaymentAsync()
    {
        var updateRecurring = false;
        if (SelectedPayment.IsRecurring)
        {
            updateRecurring = await dialogService.ShowConfirmMessageAsync(
                title: Translations.ModifyRecurrenceTitle,
                message: Translations.ModifyRecurrenceMessage,
                positiveButtonText: Translations.YesLabel,
                negativeButtonText: Translations.NoLabel);
        }

        // Due to a bug in .net maui, the loading dialog can only be called after any other dialog
        await dialogService.ShowLoadingDialogAsync(Translations.SavingPaymentMessage);
        var command = new UpdatePaymentCommand(
            id: SelectedPayment.Id,
            date: SelectedPayment.Date,
            amount: SelectedPayment.Amount,
            isCleared: SelectedPayment.IsCleared,
            type: SelectedPayment.Type,
            note: SelectedPayment.Note,
            isRecurring: SelectedPayment.IsRecurring,
            categoryId: SelectedPayment.Category?.Id ?? 0,
            chargedAccountId: SelectedPayment.ChargedAccount?.Id ?? 0,
            targetAccountId: SelectedPayment.TargetAccount?.Id ?? 0,
            updateRecurringPayment: updateRecurring,
            recurrence: SelectedPayment.RecurringPayment?.Recurrence,
            isEndless: SelectedPayment.RecurringPayment?.IsEndless,
            endDate: SelectedPayment.RecurringPayment?.EndDate,
            isLastDayOfMonth: SelectedPayment.RecurringPayment?.IsLastDayOfMonth ?? false);

        await mediator.Send(command);
    }

    private async Task DeletePaymentAsync(PaymentViewModel payment)
    {
        if (await dialogService.ShowConfirmMessageAsync(title: Translations.DeleteTitle, message: Translations.DeletePaymentConfirmationMessage))
        {
            var deleteCommand = new DeletePaymentByIdCommand(payment.Id);
            if (SelectedPayment.IsRecurring)
            {
                deleteCommand.DeleteRecurringPayment = await dialogService.ShowConfirmMessageAsync(
                    title: Translations.DeleteRecurringPaymentTitle,
                    message: Translations.DeleteRecurringPaymentMessage);
            }

            try
            {
                await dialogService.ShowLoadingDialogAsync();
                await mediator.Send(deleteCommand);
                await Shell.Current.Navigation.PopModalAsync();
            }
            finally
            {
                await dialogService.HideLoadingDialogAsync();
            }
        }
    }
}
