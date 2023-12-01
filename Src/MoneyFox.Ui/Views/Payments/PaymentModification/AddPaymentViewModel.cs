namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using Aptabase.Maui;
using Controls.CategorySelection;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features.PaymentCreation;
using Core.Features.RecurringTransactionCreation;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Resources.Strings;

internal sealed class AddPaymentViewModel : ModifyPaymentViewModel, IQueryAttributable
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;

    public AddPaymentViewModel(
        IMediator mediator,
        IDialogService dialogService,
        IToastService toastService,
        ISettingsFacade settingsFacade,
        CategorySelectionViewModel categorySelectionViewModel,
        IAptabaseClient aptabaseClient) : base(
        mediator: mediator,
        dialogService: dialogService,
        toastService: toastService,
        categorySelectionViewModel: categorySelectionViewModel,
        settingsFacade: settingsFacade,
        aptabaseClient: aptabaseClient)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
        this.settingsFacade = settingsFacade;
    }

    public new void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (IsFirstLoad)
        {
            InitializeAsync().GetAwaiter().GetResult();
            if (ChargedAccounts.Any())
            {
                SelectedPayment.ChargedAccount = settingsFacade.DefaultAccount == default
                    ? ChargedAccounts[0]
                    : ChargedAccounts.First(n => n.Id == settingsFacade.DefaultAccount);
            }
        }

        base.ApplyQueryAttributes(query);
    }

    protected override async Task SavePaymentAsync()
    {
        await dialogService.ShowLoadingDialogAsync(Translations.SavingPaymentMessage);
        var recurringTransactionId = Guid.NewGuid();
        if (SelectedPayment.IsRecurring)
        {
            var amountAdjustedForType = SelectedPayment.Type == PaymentType.Expense ? -SelectedPayment.Amount : SelectedPayment.Amount;
            await mediator.Send(
                new CreateRecurringTransaction.Command(
                    recurringTransactionId: recurringTransactionId,
                    chargedAccount: SelectedPayment.ChargedAccount.Id,
                    targetAccount: SelectedPayment.TargetAccount?.Id,
                    amount: new(amount: amountAdjustedForType, currency: SelectedPayment.ChargedAccount.CurrentBalance.Currency),
                    categoryId: CategorySelectionViewModel.SelectedCategory?.Id,
                    startDate: RecurrenceViewModel.StartDate.ToDateOnly(),
                    endDate: RecurrenceViewModel.EndDate?.ToDateOnly(),
                    recurrence: RecurrenceViewModel.Recurrence.ToRecurrence(),
                    note: SelectedPayment.Note,
                    isLastDayOfMonth: RecurrenceViewModel.IsLastDayOfMonth,
                    lastRecurrence: DateTime.Today.ToDateOnly(),
                    isTransfer: SelectedPayment.Type == PaymentType.Transfer));
        }

        await mediator.Send(
            new CreatePayment.Command(
                ChargedAccountId: SelectedPayment.ChargedAccount.Id,
                TargetAccountId: SelectedPayment.TargetAccount?.Id,
                Amount: new(amount: SelectedPayment.Amount, currency: SelectedPayment.ChargedAccount.CurrentBalance.Currency),
                Type: SelectedPayment.Type,
                Date: SelectedPayment.Date.ToDateOnly(),
                CategoryId: CategorySelectionViewModel.SelectedCategory?.Id,
                RecurringTransactionId: SelectedPayment.IsRecurring ? recurringTransactionId : null,
                Note: SelectedPayment.Note ?? string.Empty));
    }
}
