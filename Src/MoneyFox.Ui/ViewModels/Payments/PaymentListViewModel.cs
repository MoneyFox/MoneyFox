namespace MoneyFox.Ui.ViewModels.Payments;

using System.Collections.ObjectModel;
using System.Globalization;
using Accounts;
using AutoMapper;
using Common.Groups;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using MoneyFox.Core.ApplicationCore.Queries;
using MoneyFox.Core.ApplicationCore.Queries.GetPaymentsForAccountIdQuery;
using MoneyFox.Core.Common.Messages;
using MoneyFox.Core.Resources;
using Views.Payments;

internal sealed class PaymentListViewModel : BaseViewModel
{
    private readonly IMapper mapper;

    private readonly IMediator mediator;

    private bool isRunning;

    private ObservableCollection<DateListGroupCollection<PaymentViewModel>> payments = new();

    private AccountViewModel selectedAccount = new();

    public PaymentListViewModel(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        IsActive = true;
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

    public ObservableCollection<DateListGroupCollection<PaymentViewModel>> Payments
    {
        get => payments;

        private set
        {
            payments = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     List with the different recurrence types.
    ///     This has to have the same order as the enum
    /// </summary>
    public static List<PaymentRecurrence> RecurrenceList
        => new()
        {
            PaymentRecurrence.Daily,
            PaymentRecurrence.DailyWithoutWeekend,
            PaymentRecurrence.Weekly,
            PaymentRecurrence.Biweekly,
            PaymentRecurrence.Monthly,
            PaymentRecurrence.Bimonthly,
            PaymentRecurrence.Quarterly,
            PaymentRecurrence.Biannually,
            PaymentRecurrence.Yearly
        };

    public AsyncRelayCommand GoToAddPaymentCommand => new(async () => await Shell.Current.GoToAsync(Routes.AddPaymentRoute));

    public AsyncRelayCommand<PaymentViewModel> GoToEditPaymentCommand
        => new(async pvm => await Shell.Current.GoToAsync($"{Routes.EditPaymentRoute}?paymentId={pvm.Id}"));

    protected override void OnActivated()
    {
        Messenger.Register<PaymentListViewModel, ReloadMessage>(recipient: this, handler: (r, m) => OnAppearingAsync(SelectedAccount.Id));
        Messenger.Register<PaymentListViewModel, PaymentListFilterChangedMessage>(recipient: this, handler: (r, m) => LoadPaymentsByMessageAsync(m));
    }

    protected override void OnDeactivated()
    {
        Messenger.Unregister<ReloadMessage>(this);
        Messenger.Unregister<PaymentListFilterChangedMessage>(this);
    }

    public async Task OnAppearingAsync(int accountId)
    {
        SelectedAccount = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));
        await LoadPaymentsByMessageAsync(new());
    }

    public async Task LoadPaymentsByMessageAsync(PaymentListFilterChangedMessage message)
    {
        try
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;
            var paymentVms = mapper.Map<List<PaymentViewModel>>(
                await mediator.Send(
                    new GetPaymentsForAccountIdQuery(
                        accountId: SelectedAccount.Id,
                        timeRangeStart: message.TimeRangeStart,
                        timeRangeEnd: message.TimeRangeEnd,
                        isClearedFilterActive: message.IsClearedFilterActive,
                        isRecurringFilterActive: message.IsRecurringFilterActive,
                        filteredPaymentType: message.FilteredPaymentType)));

            paymentVms.ForEach(x => x.CurrentAccountId = SelectedAccount.Id);
            var dailyItems = DateListGroupCollection<PaymentViewModel>.CreateGroups(
                items: paymentVms,
                getKey: s => s.Date.ToString(format: "D", provider: CultureInfo.CurrentCulture),
                getSortKey: s => s.Date);

            dailyItems.ForEach(CalculateSubBalances);
            Payments = new(dailyItems);
        }
        finally
        {
            isRunning = false;
        }
    }

    private void CalculateSubBalances(DateListGroupCollection<PaymentViewModel> group)
    {
        group.Subtitle = string.Format(
            format: Strings.ExpenseAndIncomeTemplate,
            arg0: group.Where(x => x.Type != PaymentType.Income && x.ChargedAccount.Id == SelectedAccount.Id).Sum(x => x.Amount),
            arg1: group.Where(
                    x => x.Type == PaymentType.Income || x.Type == PaymentType.Transfer && x.TargetAccount != null && x.TargetAccount.Id == SelectedAccount.Id)
                .Sum(x => x.Amount));
    }
}
