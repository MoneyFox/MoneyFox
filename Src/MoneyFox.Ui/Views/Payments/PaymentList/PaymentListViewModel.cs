namespace MoneyFox.Ui.Views.Payments.PaymentList;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Accounts;
using AutoMapper;
using Common.Groups;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Extensions;
using Core.Common.Settings;
using Core.Queries;
using Core.Queries.GetPaymentsForAccountIdQuery;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Resources.Strings;

[QueryProperty(name: nameof(AccountId), queryId: nameof(accountId))]
internal sealed class PaymentListViewModel : BasePageViewModel, IRecipient<PaymentsChangedMessage>, IRecipient<PaymentListFilterChangedMessage>
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;

    private bool isRunning;

    private ObservableCollection<DateListGroupCollection<PaymentListItemViewModel>> payments = new();

    private AccountViewModel selectedAccount = new();

    public PaymentListViewModel(IMediator mediator, IMapper mapper, ISettingsFacade settingsFacade)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.settingsFacade = settingsFacade;
    }

    private int accountId;
    public int AccountId
    {
        get => accountId;
        set => SetProperty(ref accountId, value);
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

    public ObservableCollection<DateListGroupCollection<PaymentListItemViewModel>> Payments
    {
        get => payments;

        private set
        {
            payments = value;
            OnPropertyChanged();
        }
    }

    private ReadOnlyObservableCollection<PaymentDayGroup> paymentDayGroups;
    public ReadOnlyObservableCollection<PaymentDayGroup> PaymentDayGroups
    {
        get => paymentDayGroups;
        private set => SetProperty(ref paymentDayGroups, value);
    }

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
                .Select(g => new PaymentDayGroup(DateOnly.FromDateTime(g.Key), g.ToList()))
                .ToList();
            PaymentDayGroups = new ReadOnlyObservableCollection<PaymentDayGroup>(new ObservableCollection<PaymentDayGroup>(dailyGroupedPayments));

            var dailyItems = DateListGroupCollection<PaymentListItemViewModel>.CreateGroups(
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

    private void CalculateSubBalances(DateListGroupCollection<PaymentListItemViewModel> group)
    {
        group.Subtitle = string.Format(
            format: Translations.ExpenseAndIncomeTemplate,
            arg0: group.Where(x => x.Type != PaymentType.Income && x.ChargedAccount.Id == SelectedAccount.Id)
                .Sum(x => x.Amount)
                .FormatCurrency(settingsFacade.DefaultCurrency),
            arg1: group.Where(
                    x => x.Type == PaymentType.Income || x is { Type: PaymentType.Transfer, TargetAccount: { } } && x.TargetAccount.Id == SelectedAccount.Id)
                .Sum(x => x.Amount)
                .FormatCurrency(settingsFacade.DefaultCurrency));
    }
}
