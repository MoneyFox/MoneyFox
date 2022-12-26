namespace MoneyFox.Ui.ViewModels.Payments;

using AutoMapper;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.ApplicationCore.Queries;
using Core.Commands.Payments.CreatePayment;
using Core.Common.Interfaces;
using Core.Resources;
using JetBrains.Annotations;
using MediatR;

[UsedImplicitly]
internal sealed class AddPaymentViewModel : ModifyPaymentViewModel
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly IDialogService dialogService;

    public AddPaymentViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService) : base(
        mediator: mediator,
        mapper: mapper,
        dialogService: dialogService)
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
        await dialogService.ShowLoadingDialogAsync(Strings.SavingPaymentMessage);
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
