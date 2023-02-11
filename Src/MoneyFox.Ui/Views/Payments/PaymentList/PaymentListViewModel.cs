namespace MoneyFox.Ui.Views.Payments.PaymentList;

using System.Collections.ObjectModel;
using System.Globalization;
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

internal sealed class PaymentListViewModel : BasePageViewModel, IRecipient<PaymentsChangedMessage>, IRecipient<PaymentListFilterChangedMessage>
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;

    private bool isRunning;

    private ObservableCollection<DateListGroupCollection<PaymentViewModel>> payments = new();

    private AccountViewModel selectedAccount = new();

    public PaymentListViewModel(IMediator mediator, IMapper mapper, ISettingsFacade settingsFacade)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.settingsFacade = settingsFacade;
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

    public AsyncRelayCommand<PaymentViewModel> GoToEditPaymentCommand
        => new(async pvm => await Shell.Current.GoToAsync($"{Routes.EditPaymentRoute}?paymentId={pvm.Id}"));

    public async void Receive(PaymentListFilterChangedMessage message)
    {
        await LoadPaymentsByMessageAsync(message);
    }

    public async void Receive(PaymentsChangedMessage message)
    {
        await InitializeAsync(SelectedAccount.Id);
    }

    public async Task InitializeAsync(int accountId)
    {
        SelectedAccount = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));
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
