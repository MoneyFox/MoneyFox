namespace MoneyFox.Ui.Views.Payments.PaymentList;

using System.Collections.ObjectModel;
using Accounts.AccountModification;
using AutoMapper;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using Core.Queries.PaymentsForAccount;
using MediatR;
using PaymentModification;

internal sealed class PaymentListViewModel(IMediator mediator, IMapper mapper, INavigationService navigationService) : NavigableViewModel,
    IRecipient<PaymentListFilterChangedMessage>
{
    private bool isRunning;

    private ReadOnlyObservableCollection<PaymentDayGroup> paymentDayGroups = null!;
    private AccountViewModel selectedAccount = new();

    public AccountViewModel SelectedAccount
    {
        get => selectedAccount;

        private set
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

    public AsyncRelayCommand GoToAddPaymentCommand => new(() => navigationService.GoTo<AddPaymentViewModel>(SelectedAccount.Id));

    public AsyncRelayCommand<PaymentListItemViewModel> GoToEditPaymentCommand => new(pvm => navigationService.GoTo<EditPaymentViewModel>(pvm!.Id));

    public void Receive(PaymentListFilterChangedMessage message)
    {
        LoadPaymentsByMessageAsync(message).GetAwaiter().GetResult();
    }

    public override async Task OnNavigatedAsync(object? parameter)
    {
        var accountId = Convert.ToInt32(parameter);
        SelectedAccount = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));
        await LoadPaymentsByMessageAsync(new());
    }

    public override Task OnNavigatedBackAsync(object? parameter)
    {
        return LoadPaymentsByMessageAsync(new());
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
