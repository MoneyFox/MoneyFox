namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using Controls.CategorySelection;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features._Legacy_.Payments.CreatePayment;
using Core.Features.RecurringTransactionCreation;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.CategoryAggregate;
using MediatR;
using Resources.Strings;

internal sealed class AddPaymentViewModel : ModifyPaymentViewModel, IQueryAttributable
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;

    public AddPaymentViewModel(
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
    }

    public new void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (IsFirstLoad)
        {
            var accountId = 0;
            if (query.TryGetValue(key: "defaultChargedAccountId", value: out var defaultChargedAccountId))
            {
                accountId = Convert.ToInt32(defaultChargedAccountId);
            }

            InitializeAsync().GetAwaiter().GetResult();
            if (ChargedAccounts.Any())
            {
                SelectedPayment.ChargedAccount = accountId != 0 ? ChargedAccounts.First(n => n.Id == accountId) : ChargedAccounts[0];
            }
        }

        base.ApplyQueryAttributes(query);
    }

    protected override async Task SavePaymentAsync()
    {
        // Due to a bug in .net maui, the loading dialog can only be called after any other dialog
        await dialogService.ShowLoadingDialogAsync(Translations.SavingPaymentMessage);
        var chargedAccount = await mediator.Send(new GetAccountByIdQuery(SelectedPayment.ChargedAccount.Id));
        var targetAccount = SelectedPayment.TargetAccount != null ? await mediator.Send(new GetAccountByIdQuery(SelectedPayment.TargetAccount.Id)) : null;
        Category? category = null;
        if (CategorySelectionViewModel.SelectedCategory is not null)
        {
            category = await mediator.Send(new GetCategoryByIdQuery(CategorySelectionViewModel.SelectedCategory.Id));
        }

        var payment = new Payment(
            date: SelectedPayment.Date,
            amount: SelectedPayment.Amount,
            type: SelectedPayment.Type,
            chargedAccount: chargedAccount,
            targetAccount: targetAccount,
            category: category,
            note: SelectedPayment.Note);

        if (SelectedPayment.IsRecurring)
        {
            var recurringTransactionId = Guid.NewGuid();
            payment.AddRecurringTransactionId(recurringTransactionId);
            await mediator.Send(
                new CreateRecurringTransaction.Command(
                    recurringTransactionId: recurringTransactionId,
                    chargedAccount: SelectedPayment.ChargedAccount.Id,
                    targetAccount: SelectedPayment.TargetAccount?.Id,
                    amount: new(amount: SelectedPayment.Amount, currency: SelectedPayment.ChargedAccount.CurrentBalance.Currency),
                    categoryId: CategorySelectionViewModel.SelectedCategory?.Id,
                    startDate: RecurrenceViewModel.StartDate.ToDateOnly(),
                    endDate: RecurrenceViewModel.EndDate?.ToDateOnly(),
                    recurrence: RecurrenceViewModel.Recurrence.ToRecurrence(),
                    note: SelectedPayment.Note,
                    isLastDayOfMonth: RecurrenceViewModel.IsLastDayOfMonth,
                    isTransfer: SelectedPayment.Type == PaymentType.Transfer));
        }

        await mediator.Send(new CreatePaymentCommand(payment));
    }
}
