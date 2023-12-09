namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using Aptabase.Maui;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Controls.AccountPicker;
using Controls.CategorySelection;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features.PaymentByIdDeletions;
using Core.Features.UpdatePayment;
using Core.Queries.PaymentDataById;
using MediatR;
using Resources.Strings;

internal class EditPaymentViewModel : ModifyPaymentViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;

    public EditPaymentViewModel(
        IMediator mediator,
        IDialogService dialogService,
        IToastService toastService,
        ISettingsFacade settingsFacade,
        CategorySelectionViewModel categorySelectionViewModel,
        INavigationService navigationService,
        IAptabaseClient aptabaseClient) : base(
        mediator: mediator,
        service: dialogService,
        toastService: toastService,
        categorySelectionViewModel: categorySelectionViewModel,
        facade: settingsFacade,
        navigationService: navigationService,
        client: aptabaseClient)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
        this.settingsFacade = settingsFacade;
    }

    public AsyncRelayCommand<PaymentViewModel> DeleteCommand => new(async p => await DeletePaymentAsync(p!));

    public override async Task OnNavigatedAsync(object? parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        var paymentId = Convert.ToInt32(parameter);
        var paymentData = await mediator.Send(new GetPaymentDataById.Query(paymentId));
        if (paymentData is { IsRecurring: true, RecurrenceData: not null })
        {
            RecurrenceViewModel.Recurrence = paymentData.RecurrenceData.Recurrence.ToPaymentRecurrence();
            RecurrenceViewModel.StartDate = paymentData.RecurrenceData.StartDate.ToDateTime(TimeOnly.MinValue);
            RecurrenceViewModel.EndDate = paymentData.RecurrenceData.EndDate?.ToDateTime(TimeOnly.MinValue);
            RecurrenceViewModel.IsEndless = paymentData.RecurrenceData.IsEndless;
        }

        var targetAccountPickerViewModel = paymentData.TargetAccount == null
            ? null
            : new AccountPickerViewModel(
                Id: paymentData.TargetAccount.Id,
                Name: paymentData.TargetAccount.Name,
                CurrentBalance: new(amount: paymentData.TargetAccount.CurrentBalance, currencyAlphaIsoCode: settingsFacade.DefaultCurrency));

        SelectedPayment = new()
        {
            Id = paymentData.PaymentId,
            Amount = paymentData.Amount,
            ChargedAccount
                = new(
                    Id: paymentData.ChargedAccount.Id,
                    Name: paymentData.ChargedAccount.Name,
                    CurrentBalance: new(amount: paymentData.ChargedAccount.CurrentBalance, currencyAlphaIsoCode: settingsFacade.DefaultCurrency)),
            TargetAccount = targetAccountPickerViewModel,
            Date = paymentData.Date,
            IsCleared = paymentData.IsCleared,
            Type = paymentData.Type,
            IsRecurring = paymentData.IsRecurring,
            Note = paymentData.Note,
            Created = paymentData.Created,
            LastModified = paymentData.LastModified
        };

        if (paymentData.Category != null)
        {
            CategorySelectionViewModel.SelectedCategory = new()
            {
                Id = paymentData.Category.Id, Name = paymentData.Category.Name, RequireNote = paymentData.Category.RequireNote
            };
        }
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
            Type: SelectedPayment.Type,
            Note: SelectedPayment.Note,
            IsRecurring: SelectedPayment.IsRecurring,
            CategoryId: CategorySelectionViewModel.SelectedCategory?.Id ?? 0,
            ChargedAccountId: SelectedPayment.ChargedAccount.Id,
            TargetAccountId: SelectedPayment.TargetAccount?.Id ?? 0,
            UpdateRecurringPayment: updateRecurring,
            Recurrence: RecurrenceViewModel.Recurrence,
            EndDate: RecurrenceViewModel.EndDate,
            IsLastDayOfMonth: RecurrenceViewModel.IsLastDayOfMonth);

        await mediator.Send(command);
    }

    private async Task DeletePaymentAsync(PaymentViewModel payment)
    {
        if (await dialogService.ShowConfirmMessageAsync(title: Translations.DeleteTitle, message: Translations.DeletePaymentConfirmationMessage))
        {
            var deleteRecurringPayment = false;
            if (SelectedPayment.IsRecurring)
            {
                deleteRecurringPayment = await dialogService.ShowConfirmMessageAsync(
                    title: Translations.DeleteRecurringPaymentTitle,
                    message: Translations.DeleteRecurringPaymentMessage);
            }

            DeletePaymentById.Command command = new(PaymentId: payment.Id, DeleteRecurringPayment: deleteRecurringPayment);
            try
            {
                await dialogService.ShowLoadingDialogAsync();
                await mediator.Send(command);
                await Shell.Current.Navigation.PopModalAsync();
            }
            finally
            {
                await dialogService.HideLoadingDialogAsync();
            }
        }
    }
}
