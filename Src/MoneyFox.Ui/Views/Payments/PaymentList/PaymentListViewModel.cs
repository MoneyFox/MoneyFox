namespace MoneyFox.Ui.Views.Payments.PaymentList;

using System.Collections.ObjectModel;
using Accounts.AccountModification;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using Core.Queries.GetPaymentsForAccountIdQuery;
using Domain.Aggregates.AccountAggregate;
using MediatR;

[QueryProperty(name: nameof(AccountId), queryId: nameof(accountId))]
internal sealed class PaymentListViewModel : BasePageViewModel, IRecipient<PaymentsChangedMessage>, IRecipient<PaymentListFilterChangedMessage>
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    private int accountId;

    private bool isRunning;

    private ReadOnlyObservableCollection<PaymentDayGroup> paymentDayGroups = null!;
    private AccountViewModel selectedAccount = new();

    public PaymentListViewModel(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public int AccountId
    {
        get => accountId;
        set => SetProperty(field: ref accountId, newValue: value);
    }

    public AccountViewModel SelectedAccount
    {
        get => selectedAccount;

        set
        {
            selectedAccount = value;
            OnPropertyChanged();
        }
    }

    public ReadOnlyObservableCollection<PaymentDayGroup> PaymentDayGroups
    {
        get => paymentDayGroups;
        private set => SetProperty(field: ref paymentDayGroups, newValue: value);
    }

    public AsyncRelayCommand GoToAddPaymentCommand
        => new(async () => await Shell.Current.GoToAsync($"{Routes.AddPaymentRoute}?defaultChargedAccountId={SelectedAccount.Id}"));

    public AsyncRelayCommand<PaymentListItemViewModel> GoToEditPaymentCommand
        => new(async pvm => await Shell.Current.GoToAsync($"{Routes.EditPaymentRoute}?paymentId={pvm.Id}"));

    public void Receive(PaymentListFilterChangedMessage message)
    {
        LoadPaymentsByMessageAsync(message).GetAwaiter().GetResult();
    }

    public void Receive(PaymentsChangedMessage message)
    {
        InitializeAsync().GetAwaiter().GetResult();
    }

    public async Task InitializeAsync()
    {
        IsActive = true;
        SelectedAccount = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(AccountId)));
        await LoadPaymentsByMessageAsync(new());
    }

    private async Task LoadPaymentsByMessageAsync(PaymentListFilterChangedMessage message)
    {
        try
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;
            var paymentVms = mapper.Map<List<PaymentListItemViewModel>>(
                await mediator.Send(
                    new GetPaymentsForAccountIdQuery(
                        accountId: SelectedAccount.Id,
                        timeRangeStart: message.TimeRangeStart,
                        timeRangeEnd: message.TimeRangeEnd,
                        isClearedFilterActive: message.IsClearedFilterActive,
                        isRecurringFilterActive: message.IsRecurringFilterActive,
                        filteredPaymentType: message.FilteredPaymentType)));

            paymentVms.ForEach(x => x.CurrentAccountId = SelectedAccount.Id);
            var dailyGroupedPayments = paymentVms.GroupBy(p => p.Date.Date)
                .Select(g => new PaymentDayGroup(date: DateOnly.FromDateTime(g.Key), payments: g.ToList()))
                .ToList();

            PaymentDayGroups = new(new(dailyGroupedPayments));
        }
        finally
        {
            isRunning = false;
        }
    }
}
