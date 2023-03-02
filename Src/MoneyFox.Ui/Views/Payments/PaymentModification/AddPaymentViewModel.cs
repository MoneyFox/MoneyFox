namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Payments.CreatePayment;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.CategoryAggregate;
using MediatR;
using MoneyFox.Ui.Controls.CategorySelection;
using Resources.Strings;

internal sealed class AddPaymentViewModel : ModifyPaymentViewModel, IQueryAttributable
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;

    public AddPaymentViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, IToastService toastService) : base(
        mediator: mediator,
        mapper: mapper,
        dialogService: dialogService,
        toastService: toastService)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        int accountId = 0;
        if (query.TryGetValue("defaultChargedAccountId", out var defaultChargedAccountId))
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

    protected override async Task SavePaymentAsync()
    {
        // Due to a bug in .net maui, the loading dialog can only be called after any other dialog
        await dialogService.ShowLoadingDialogAsync(Translations.SavingPaymentMessage);
        var chargedAccount = await mediator.Send(new GetAccountByIdQuery(SelectedPayment.ChargedAccount.Id));
        var targetAccount = SelectedPayment.TargetAccount != null ? await mediator.Send(new GetAccountByIdQuery(SelectedPayment.TargetAccount.Id)) : null;

        int? selectedCategoryId = WeakReferenceMessenger.Default.Send<SelectedCategoryRequestMessage>();

        Category? category = null;
        if (selectedCategoryId is not null)
        {
            category = await mediator.Send(new GetCategoryByIdQuery(selectedCategoryId.Value));
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
