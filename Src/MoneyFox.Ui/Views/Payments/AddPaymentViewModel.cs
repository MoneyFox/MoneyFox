namespace MoneyFox.Ui.Views.Payments;

using AutoMapper;
using MediatR;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using MoneyFox.Core.ApplicationCore.Queries;
using MoneyFox.Core.Commands.Payments.CreatePayment;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Ui.Resources.Strings;

internal sealed class AddPaymentViewModel : ModifyPaymentViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public AddPaymentViewModel(
        IMediator mediator,
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
    }

    public async Task InitializeAsync(int? defaultChargedAccountId = null)
    {
        if (IsFirstLoad is false)
        {
            return;
        }

        await base.InitializeAsync();
        if (ChargedAccounts.Any())
        {
            SelectedPayment.ChargedAccount = defaultChargedAccountId.HasValue
                ? ChargedAccounts.First(n => n.Id == defaultChargedAccountId.Value)
                : ChargedAccounts.First();
        }

        IsFirstLoad = false;
    }

    protected override async Task SavePaymentAsync()
    {
        // Due to a bug in .net maui, the loading dialog can only be called after any other dialog
        await dialogService.ShowLoadingDialogAsync(Translations.SavingPaymentMessage);
        var chargedAccount = await mediator.Send(new GetAccountByIdQuery(SelectedPayment.ChargedAccount.Id));
        var targetAccount = SelectedPayment.TargetAccount != null ? await mediator.Send(new GetAccountByIdQuery(SelectedPayment.TargetAccount.Id)) : null;
        var payment = new Payment(
            date: SelectedPayment.Date,
            amount: SelectedPayment.Amount,
            type: SelectedPayment.Type,
            chargedAccount: chargedAccount,
            targetAccount: targetAccount,
            category: mapper.Map<Category>(SelectedPayment.Category),
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
