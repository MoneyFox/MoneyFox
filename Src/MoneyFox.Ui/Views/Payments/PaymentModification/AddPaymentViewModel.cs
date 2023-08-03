namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using AutoMapper;
using Controls.CategorySelection;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features._Legacy_.Payments.CreatePayment;
using Core.Features.RecurringTransactionCreation;
using Core.Queries;
using Domain.Aggregates;
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
        IMapper mapper,
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

        if (SelectedPayment.IsRecurring)
        {
            await mediator.Send(
                new CreateRecurringTransaction.Command(
                    RecurringTransactionId: Guid.NewGuid(),
                    ChargedAccount: SelectedPayment.ChargedAccount.Id,
                    TargetAccount: SelectedPayment.TargetAccount?.Id,
                    Amount: new(amount: SelectedPayment.Amount, currency: SelectedPayment.ChargedAccount.CurrentBalance.Currency),
                    CategoryId: CategorySelectionViewModel.SelectedCategory?.Id,
                    StartDate: RecurrenceViewModel.StartDate.ToDateOnly(),
                    EndDate: RecurrenceViewModel.EndDate?.ToDateOnly(),
                    Recurrence: RecurrenceViewModel.Recurrence.ToRecurrence(),
                    Note: SelectedPayment.Note,
                    IsLastDayOfMonth: RecurrenceViewModel.IsLastDayOfMonth,
                    IsTransfer: SelectedPayment.Type == PaymentType.Transfer));
        }

        var payment = new Payment(
            date: SelectedPayment.Date,
            amount: SelectedPayment.Amount,
            type: SelectedPayment.Type,
            chargedAccount: chargedAccount,
            targetAccount: targetAccount,
            category: category,
            note: SelectedPayment.Note);

        await mediator.Send(new CreatePaymentCommand(payment));
    }
}

public static class RecurringTransactionExtensions
{
    public static Recurrence ToRecurrence(this PaymentRecurrence recurrence)
    {
        return recurrence switch
        {
            PaymentRecurrence.Daily => Recurrence.Daily,
            PaymentRecurrence.DailyWithoutWeekend => Recurrence.DailyWithoutWeekend,
            PaymentRecurrence.Weekly => Recurrence.Weekly,
            PaymentRecurrence.Biweekly => Recurrence.Biweekly,
            PaymentRecurrence.Monthly => Recurrence.Monthly,
            PaymentRecurrence.Bimonthly => Recurrence.Bimonthly,
            PaymentRecurrence.Quarterly => Recurrence.Quarterly,
            PaymentRecurrence.Yearly => Recurrence.Quarterly,
            PaymentRecurrence.Biannually => Recurrence.Biannually,
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(recurrence), actualValue: recurrence, message: null)
        };
    }
}
