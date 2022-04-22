namespace MoneyFox.Win.ViewModels.Payments;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core._Pending_.Common.Facades;
using Core._Pending_.Common.Messages;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using Core.Commands.Payments.DeletePaymentById;
using Core.Common.Interfaces;
using Core.Resources;
using Groups;
using Interfaces;
using MediatR;
using Microsoft.UI.Xaml.Data;
using Serilog;
using Services;

public class PaymentListViewModel : ObservableRecipient
{
    private const int DEFAULT_YEAR_BACK = -2;

    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IBalanceCalculationService balanceCalculationService;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ISettingsFacade settingsFacade;

    private int accountId;
    private IBalanceViewModel balanceViewModel = null!;

    private string title = "";
    private bool isBusy;
    private IPaymentListViewActionViewModel? viewActionViewModel;

    private CollectionViewSource? groupedPayments;

    /// <summary>
    ///     Default constructor
    /// </summary>
    public PaymentListViewModel(
        IMediator mediator,
        IMapper mapper,
        IDialogService dialogService,
        ISettingsFacade settingsFacade,
        IBalanceCalculationService balanceCalculationService,
        INavigationService navigationService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
        this.settingsFacade = settingsFacade;
        this.balanceCalculationService = balanceCalculationService;
        this.navigationService = navigationService;
    }

    public List<PaymentTypeFilter> PaymentTypeFilterList
        => new()
        {
            PaymentTypeFilter.All,
            PaymentTypeFilter.Expense,
            PaymentTypeFilter.Income,
            PaymentTypeFilter.Transfer
        };

    public AsyncRelayCommand InitializeCommand => new(async () => await InitializeAsync());

    public AsyncRelayCommand LoadDataCommand => new(async () => await LoadDataAsync(new() { TimeRangeStart = DateTime.Now.AddYears(DEFAULT_YEAR_BACK) }));

    public RelayCommand<PaymentViewModel> EditPaymentCommand => new(vm => navigationService.Navigate<EditPaymentViewModel>(vm));

    /// <summary>
    ///     Deletes the passed PaymentViewModel.
    /// </summary>
    public RelayCommand<PaymentViewModel> DeletePaymentCommand => new(async vm => await DeletePaymentAsync(vm));

    /// <summary>
    ///     Id for the current account.
    /// </summary>
    public int AccountId
    {
        get => accountId;
        set => SetProperty(field: ref accountId, newValue: value);
    }

    /// <summary>
    ///     View Model for the balance subview.
    /// </summary>
    public IBalanceViewModel BalanceViewModel
    {
        get => balanceViewModel;
        private set => SetProperty(field: ref balanceViewModel, newValue: value);
    }

    /// <summary>
    ///     View Model for the global actions on the view.
    /// </summary>
    public IPaymentListViewActionViewModel? ViewActionViewModel
    {
        get => viewActionViewModel;
        private set => SetProperty(field: ref viewActionViewModel, newValue: value);
    }

    /// <summary>
    ///     Returns grouped related payments
    /// </summary>
    public CollectionViewSource? GroupedPayments
    {
        get => groupedPayments;
        private set => SetProperty(field: ref groupedPayments, newValue: value);
    }

    /// <summary>
    ///     Returns the name of the account title for the current page
    /// </summary>
    public string Title
    {
        get => title;
        private set => SetProperty(field: ref title, newValue: value);
    }

    public bool IsBusy
    {
        get => isBusy;
        private set => SetProperty(field: ref isBusy, newValue: value);
    }

    protected override void OnActivated()
    {
        Messenger.Register<PaymentListViewModel, PaymentListFilterChangedMessage>(recipient: this, handler: (r, m) => r.LoadDataAsync(m));
        Messenger.Register<PaymentListViewModel, ReloadMessage>(recipient: this, handler: (r, m) => r.LoadDataCommand.Execute(null));
    }

    protected override void OnDeactivated()
    {
        Messenger.Unregister<PaymentListFilterChangedMessage>(this);
        Messenger.Unregister<ReloadMessage>(this);
    }

    private async Task InitializeAsync()
    {
        IsActive = true;
        IsBusy = true;
        Title = await mediator.Send(new GetAccountNameByIdQuery(accountId));
        BalanceViewModel = new PaymentListBalanceViewModel(
            mediator: mediator,
            mapper: mapper,
            balanceCalculationService: balanceCalculationService,
            accountId: AccountId);

        ViewActionViewModel = new PaymentListViewActionViewModel(
            accountId: AccountId,
            mediator: mediator,
            settingsFacade: settingsFacade,
            dialogService: dialogService,
            balanceViewModel: BalanceViewModel,
            navigationService: navigationService);

        await LoadDataAsync(new() { TimeRangeStart = DateTime.Now.AddYears(DEFAULT_YEAR_BACK) });
        IsBusy = false;
    }

    private async Task LoadDataAsync(PaymentListFilterChangedMessage paymentListFilterChangedMessage)
    {
        if (AccountId == 0)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await LoadPaymentsAsync(paymentListFilterChangedMessage);

            //Refresh balance control with the current account
            await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync(null);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadPaymentsAsync(PaymentListFilterChangedMessage filterMessage)
    {
        var getPaymentsForAccountIdQuery = new GetPaymentsForAccountIdQuery(
            accountId: AccountId,
            timeRangeStart: filterMessage.TimeRangeStart,
            timeRangeEnd: filterMessage.TimeRangeEnd,
            isClearedFilterActive: filterMessage.IsClearedFilterActive,
            isRecurringFilterActive: filterMessage.IsRecurringFilterActive,
            filteredPaymentType: filterMessage.FilteredPaymentType);

        var loadedPayments = await mediator.Send(getPaymentsForAccountIdQuery);
        var payments = mapper.Map<List<PaymentViewModel>>(loadedPayments);
        payments.ForEach(x => x.CurrentAccountId = AccountId);
        var source = new CollectionViewSource { IsSourceGrouped = filterMessage.IsGrouped };
        if (filterMessage.IsGrouped)
        {
            var group = DateListGroupCollection<PaymentViewModel>.CreateGroups(
                items: payments,
                getKey: s => s.Date.ToString(format: "D", provider: CultureInfo.CurrentCulture),
                getSortKey: s => s.Date);

            source.Source = group;
        }
        else
        {
            source.Source = payments;
        }

        GroupedPayments = source;
    }

    private async Task DeletePaymentAsync(PaymentViewModel payment)
    {
        if (!await dialogService.ShowConfirmMessageAsync(
                title: Strings.DeleteTitle,
                message: Strings.DeletePaymentConfirmationMessage,
                positiveButtonText: Strings.YesLabel,
                negativeButtonText: Strings.NoLabel))
        {
            return;
        }

        try
        {
            IsBusy = true;
            var command = new DeletePaymentByIdCommand(payment.Id);
            if (payment.IsRecurring)
            {
                command.DeleteRecurringPayment = await dialogService.ShowConfirmMessageAsync(
                    title: Strings.DeleteRecurringPaymentTitle,
                    message: Strings.DeleteRecurringPaymentMessage);
            }

            await mediator.Send(command);
            Messenger.Send(new ReloadMessage());
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Error during deleting payment");
            await dialogService.ShowMessageAsync(title: Strings.GeneralErrorTitle, message: Strings.UnknownErrorMessage);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
