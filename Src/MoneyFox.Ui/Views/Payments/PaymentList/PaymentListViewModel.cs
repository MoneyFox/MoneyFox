namespace MoneyFox.Ui.Views.Payments.PaymentList;

using System.Collections.ObjectModel;
using Accounts.AccountModification;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using Core.Queries.PaymentsForAccount;
using MediatR;
using PaymentModification;

[QueryProperty(name: nameof(AccountId), queryId: nameof(accountId))]
internal sealed class PaymentListViewModel(IMediator mediator, IMapper mapper, INavigationService navigationService) : BasePageViewModel,
    IRecipient<PaymentsChangedMessage>,
    IRecipient<PaymentListFilterChangedMessage>
{
    private int accountId;

    private bool isRunning;

    private ReadOnlyObservableCollection<PaymentDayGroup> paymentDayGroups = null!;
    private AccountViewModel selectedAccount = new();

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
        => new(() => navigationService.GoTo<AddPaymentViewModel>(SelectedAccount.Id));

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
            var paymentData = await mediator.Send(
                new GetPaymentsForAccount.Query(
                    AccountId: SelectedAccount.Id,
                    TimeRangeStart: message.TimeRangeStart,
                    TimeRangeEnd: message.TimeRangeEnd,
                    FilteredPaymentType: message.FilteredPaymentType,
                    IsRecurringFilterActive: message.IsClearedFilterActive,
                    IsClearedFilterActive: message.IsRecurringFilterActive));

            var paymentVms = paymentData.Select(
                    p => new PaymentListItemViewModel
                    {
                        Id = p.Id,
                        Amount = p.Amount,
                        ChargedAccountId = p.ChargedAccountId,
                        CurrentAccountId = SelectedAccount.Id,
                        Date = p.Date.ToDateTime(TimeOnly.MinValue),
                        CategoryName = p.CategoryName,
                        IsCleared = p.IsCleared,
                        IsRecurring = p.IsRecurring,
                        Note = p.Note,
                        Type = p.Type
                    })
                .OrderByDescending(p => p.Date);

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
