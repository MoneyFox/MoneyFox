namespace MoneyFox.Ui.Views.Payments.PaymentList;

using System.Collections.ObjectModel;
using Accounts.AccountModification;
using Common.Navigation;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using Core.Queries.PaymentsForAccount;
using MediatR;
using PaymentModification;

internal sealed class PaymentListViewModel : NavigableViewModel
{
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;
    private readonly IPopupService popupService;
    private PaymentListFilterChangedMessage? filterChangedMessage;
    private ReadOnlyObservableCollection<PaymentDayGroup> paymentDayGroups = null!;
    private GetAccountById.AccountData selectedAccount;

    public PaymentListViewModel(IMediator mediator, INavigationService navigationService, IPopupService popupService)
    {
        this.mediator = mediator;
        this.navigationService = navigationService;
        this.popupService = popupService;
        WeakReferenceMessenger.Default.Register<PaymentListFilterChangedMessage>(
            recipient: this,
            handler: (_, m) =>
            {
                filterChangedMessage = m;
                LoadPayments(m).GetAwaiter().GetResult();
            });
    }

    public GetAccountById.AccountData SelectedAccount
    {
        get => selectedAccount;
        set => SetProperty(field: ref selectedAccount, newValue: value);
    }


    public ReadOnlyObservableCollection<PaymentDayGroup> PaymentDayGroups
    {
        get => paymentDayGroups;
        private set => SetProperty(field: ref paymentDayGroups, newValue: value);
    }

    public AsyncRelayCommand ShowFilterCommand
        => new(() => popupService.ShowPopupAsync<SelectFilterPopupViewModel>(vm => { vm.Initialize(filterChangedMessage); }));

    public AsyncRelayCommand GoToAddPaymentCommand => new(() => navigationService.GoTo<AddPaymentViewModel>(SelectedAccount.AccountId));

    public AsyncRelayCommand<PaymentListItemViewModel> GoToEditPaymentCommand => new(pvm => navigationService.GoTo<EditPaymentViewModel>(pvm!.Id));

    public override async Task OnNavigatedAsync(object? parameter)
    {
        var accountId = Convert.ToInt32(parameter);
        SelectedAccount = await mediator.Send(new GetAccountById.Query(accountId));
        await LoadPayments(new());
    }

    public override Task OnNavigatedBackAsync(object? parameter)
    {
        return LoadPayments(new());
    }

    private async Task LoadPayments(PaymentListFilterChangedMessage message)
    {
        var paymentData = await mediator.Send(
            new GetPaymentsForAccount.Query(
                AccountId: SelectedAccount.AccountId,
                TimeRangeStart: message.TimeRangeStart,
                TimeRangeEnd: message.TimeRangeEnd,
                FilteredPaymentType: message.FilteredPaymentType,
                IsClearedFilterActive: message.IsClearedFilterActive,
                IsRecurringFilterActive: message.IsRecurringFilterActive));

        var paymentVms = paymentData.Select(
                p => new PaymentListItemViewModel
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    ChargedAccountId = p.ChargedAccountId,
                    CurrentAccountId = SelectedAccount.AccountId,
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
}
