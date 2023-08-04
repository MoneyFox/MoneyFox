namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using CommunityToolkit.Mvvm.Input;
using Controls.AccountPicker;
using Controls.CategorySelection;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features._Legacy_.Payments.DeletePaymentById;
using Core.Features._Legacy_.Payments.UpdatePayment;
using Core.Queries;
using MediatR;
using Resources.Strings;

internal class EditPaymentViewModel : ModifyPaymentViewModel, IQueryAttributable
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;

    public EditPaymentViewModel(
        IMediator mediator,
        IDialogService dialogService,
        IToastService toastService,
        ISettingsFacade settingsFacade,
        CategorySelectionViewModel categorySelectionViewModel) : base(
        mediator: mediator,
        dialogService: dialogService,
        toastService: toastService,
        categorySelectionViewModel: categorySelectionViewModel,
        settingsFacade: settingsFacade)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
        this.settingsFacade = settingsFacade;
    }

    public AsyncRelayCommand<PaymentViewModel> DeleteCommand => new(async p => await DeletePaymentAsync(p));

    public new void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(key: "paymentId", value: out var paymentIdParam))
        {
            var paymentId = Convert.ToInt32(paymentIdParam);
            InitializeAsync(paymentId).GetAwaiter().GetResult();
        }

        base.ApplyQueryAttributes(query);
    }

    public async Task InitializeAsync(int paymentId)
    {
        if (IsFirstLoad is false)
        {
            return;
        }

        await InitializeAsync();
        var payment = await mediator.Send(new GetPaymentByIdQuery(paymentId));
        if (payment is { IsRecurring: true, RecurringPayment: not null })
        {
            RecurrenceViewModel.Recurrence = payment.RecurringPayment.Recurrence;
            RecurrenceViewModel.StartDate = payment.RecurringPayment.StartDate;
            RecurrenceViewModel.EndDate = payment.RecurringPayment.EndDate;
            RecurrenceViewModel.IsEndless = payment.RecurringPayment.IsEndless;
        }

        var targetAccountPickerViewModel = payment.TargetAccount == null
            ? null
            : new AccountPickerViewModel(
                Id: payment.TargetAccount.Id,
                Name: payment.TargetAccount.Name,
                CurrentBalance: new(amount: payment.TargetAccount.CurrentBalance, currencyAlphaIsoCode: settingsFacade.DefaultCurrency));

        SelectedPayment = new()
        {
            Id = payment.Id,
            Amount = payment.Amount,
            ChargedAccount
                = new(
                    Id: payment.ChargedAccount.Id,
                    Name: payment.ChargedAccount.Name,
                    CurrentBalance: new(amount: payment.ChargedAccount.CurrentBalance, currencyAlphaIsoCode: settingsFacade.DefaultCurrency)),
            TargetAccount = targetAccountPickerViewModel,
            Date = payment.Date,
            IsCleared = payment.IsCleared,
            Type = payment.Type,
            IsRecurring = payment.IsRecurring,
            Note = payment.Note,
            Created = payment.Created,
            LastModified = payment.LastModified
        };

        if (payment.Category != null)
        {
            CategorySelectionViewModel.SelectedCategory
                = new() { Id = payment.Category.Id, Name = payment.Category.Name, RequireNote = payment.Category.RequireNote };
        }

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
            Type: SelectedPayment.Type,
            Note: SelectedPayment.Note,
            IsRecurring: SelectedPayment.IsRecurring,
            CategoryId: CategorySelectionViewModel.SelectedCategory?.Id ?? 0,
            ChargedAccountId: SelectedPayment.ChargedAccount?.Id ?? 0,
            TargetAccountId: SelectedPayment.TargetAccount?.Id ?? 0,
            UpdateRecurringPayment: updateRecurring,
            Recurrence: RecurrenceViewModel.Recurrence,
            IsEndless: RecurrenceViewModel.IsEndless,
            EndDate: RecurrenceViewModel.EndDate,
            IsLastDayOfMonth: RecurrenceViewModel.IsLastDayOfMonth);

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
