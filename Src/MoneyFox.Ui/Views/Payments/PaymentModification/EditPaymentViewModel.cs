namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Features._Legacy_.Payments.DeletePaymentById;
using MoneyFox.Core.Features._Legacy_.Payments.UpdatePayment;
using MoneyFox.Core.Queries;
using MoneyFox.Ui.Resources.Strings;

internal class EditPaymentViewModel : ModifyPaymentViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public EditPaymentViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, IToastService toastService) : base(
        mediator: mediator,
        mapper: mapper,
        dialogService: dialogService,
        toastService: toastService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
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
        var command = new UpdatePayment.Command(
            Id: SelectedPayment.Id,
            Date: SelectedPayment.Date,
            Amount: SelectedPayment.Amount,
            IsCleared: SelectedPayment.IsCleared,
            Type: SelectedPayment.Type,
            Note: SelectedPayment.Note,
            IsRecurring: SelectedPayment.IsRecurring,
            CategoryId: SelectedPayment.Category?.Id ?? 0,
            ChargedAccountId: SelectedPayment.ChargedAccount?.Id ?? 0,
            TargetAccountId: SelectedPayment.TargetAccount?.Id ?? 0,
            UpdateRecurringPayment: updateRecurring,
            Recurrence: SelectedPayment.RecurringPayment?.Recurrence,
            IsEndless: SelectedPayment.RecurringPayment?.IsEndless,
            EndDate: SelectedPayment.RecurringPayment?.EndDate,
            IsLastDayOfMonth: SelectedPayment.RecurringPayment?.IsLastDayOfMonth ?? false);

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