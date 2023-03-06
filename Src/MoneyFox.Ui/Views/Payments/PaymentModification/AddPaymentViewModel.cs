namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using AutoMapper;
using Controls.CategorySelection;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Payments.CreatePayment;
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
        IMapper mapper,
        IDialogService dialogService,
        IToastService toastService,
        CategorySelectionViewModel categorySelectionViewModel) : base(
        mediator: mediator,
        mapper: mapper,
        dialogService: dialogService,
        toastService: toastService,
        categorySelectionViewModel: categorySelectionViewModel)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
    }

    public new void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (IsFirstLoad)
        {
            var accountId = 0;
            if (query.TryGetValue(key: "defaultChargedAccountId", out var defaultChargedAccountId))
            {
                accountId = Convert.ToInt32(defaultChargedAccountId);
            }

            InitializeAsync().GetAwaiter().GetResult();
            if (ChargedAccounts.Any())
            {
                SelectedPayment.ChargedAccount = accountId != 0
                    ? ChargedAccounts.First(n => n.Id == accountId)
                    : ChargedAccounts.First();
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

        if (SelectedPayment.IsRecurring && SelectedPayment.RecurringPayment != null)
        {
            payment.AddRecurringPayment(
                recurrence: SelectedPayment.RecurringPayment.Recurrence,
                isLastDayOfMonth: SelectedPayment.RecurringPayment.IsLastDayOfMonth,
                endDate: SelectedPayment.RecurringPayment.IsEndless ? null : SelectedPayment.RecurringPayment.EndDate);
        }

        await mediator.Send(new CreatePaymentCommand(payment));
    }
}
