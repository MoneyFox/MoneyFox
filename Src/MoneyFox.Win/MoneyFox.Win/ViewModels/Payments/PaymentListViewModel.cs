namespace MoneyFox.Win.ViewModels.Payments;

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core._Pending_.Common.Facades;
using Core._Pending_.Common.Messages;
using Core.Aggregates.Payments;
using Core.Commands.Payments.DeletePaymentById;
using Core.Common.Interfaces;
using Core.Queries.Accounts.GetAccountNameById;
using Core.Queries.Payments.GetPaymentsForAccountId;
using Core.Resources;
using Groups;
using Interfaces;
using MediatR;
using Microsoft.UI.Xaml.Data;
using NLog;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

/// <summary>
///     Representation of the payment list view.
/// </summary>
public class PaymentListViewModel : ObservableRecipient
{
    private readonly Logger logManager = LogManager.GetCurrentClassLogger();

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

    public List<PaymentTypeFilter> PaymentTypeFilterList => new() { PaymentTypeFilter.All, PaymentTypeFilter.Expense, PaymentTypeFilter.Income, PaymentTypeFilter.Transfer };


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

    protected override void OnActivated()
    {
        Messenger.Register<PaymentListViewModel, PaymentListFilterChangedMessage>(
            this,
            (r, m)
                => r.LoadDataAsync(m));
        Messenger.Register<PaymentListViewModel, ReloadMessage>(
            this,
            (r, m)
                => r.LoadDataCommand.Execute(null));
    }

    protected override void OnDeactivated()
    {
        Messenger.Unregister<PaymentListFilterChangedMessage>(this);
        Messenger.Unregister<ReloadMessage>(this);
    }

    public AsyncRelayCommand InitializeCommand => new(async () => await InitializeAsync());

    public AsyncRelayCommand LoadDataCommand => new(
        async () => await LoadDataAsync(
            new PaymentListFilterChangedMessage { TimeRangeStart = DateTime.Now.AddYears(DEFAULT_YEAR_BACK) }));

    public RelayCommand<PaymentViewModel> EditPaymentCommand
        => new(vm => navigationService.Navigate<EditPaymentViewModel>(vm));

    /// <summary>
    ///     Deletes the passed PaymentViewModel.
    /// </summary>
    public RelayCommand<PaymentViewModel> DeletePaymentCommand =>
        new(async vm => await DeletePaymentAsync(vm));

    /// <summary>
    ///     Id for the current account.
    /// </summary>
    public int AccountId
    {
        get => accountId;
        set => SetProperty(ref accountId, value);
    }

    /// <summary>
    ///     View Model for the balance subview.
    /// </summary>
    public IBalanceViewModel BalanceViewModel
    {
        get => balanceViewModel;
        private set => SetProperty(ref balanceViewModel, value);
    }

    /// <summary>
    ///     View Model for the global actions on the view.
    /// </summary>
    public IPaymentListViewActionViewModel? ViewActionViewModel
    {
        get => viewActionViewModel;
        private set => SetProperty(ref viewActionViewModel, value);
    }

    private CollectionViewSource? groupedPayments;

    /// <summary>
    ///     Returns grouped related payments
    /// </summary>
    public CollectionViewSource? GroupedPayments
    {
        get => groupedPayments;
        private set => SetProperty(ref groupedPayments, value);
    }

    /// <summary>
    ///     Returns the name of the account title for the current page
    /// </summary>
    public string Title
    {
        get => title;
        private set => SetProperty(ref title, value);
    }

    public bool IsBusy
    {
        get => isBusy;
        private set => SetProperty(ref isBusy, value);
    }

    private async Task InitializeAsync()
    {
        IsActive = true;
        IsBusy = true;

        Title = await mediator.Send(new GetAccountNameByIdQuery(accountId));

        BalanceViewModel = new PaymentListBalanceViewModel(
            mediator,
            mapper,
            balanceCalculationService,
            AccountId);
        ViewActionViewModel = new PaymentListViewActionViewModel(
            AccountId,
            mediator,
            settingsFacade,
            dialogService,
            BalanceViewModel,
            navigationService);

        await LoadDataAsync(
            new PaymentListFilterChangedMessage { TimeRangeStart = DateTime.Now.AddYears(DEFAULT_YEAR_BACK) });
        IsBusy = false;
    }

    private async Task LoadDataAsync(PaymentListFilterChangedMessage paymentListFilterChangedMessage)
    {
        if(AccountId == 0)
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
        catch(Exception ex)
        {
            logManager.Error(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadPaymentsAsync(PaymentListFilterChangedMessage filterMessage)
    {
        var getPaymentsForAccountIdQuery = new GetPaymentsForAccountIdQuery(
            AccountId,
            filterMessage.TimeRangeStart,
            filterMessage.TimeRangeEnd,
            filterMessage.IsClearedFilterActive,
            filterMessage.IsRecurringFilterActive,
            filterMessage.FilteredPaymentType
            );

        List<Payment> loadedPayments = await mediator.Send(getPaymentsForAccountIdQuery);
        var payments = mapper.Map<List<PaymentViewModel>>(loadedPayments);

        payments.ForEach(x => x.CurrentAccountId = AccountId);

        var source = new CollectionViewSource { IsSourceGrouped = filterMessage.IsGrouped };

        if(filterMessage.IsGrouped)
        {
            List<DateListGroupCollection<PaymentViewModel>> group = DateListGroupCollection<PaymentViewModel>
                .CreateGroups(
                    payments,
                    s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                    s => s.Date);
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
        if(!await dialogService.ShowConfirmMessageAsync(
               Strings.DeleteTitle,
               Strings.DeletePaymentConfirmationMessage,
               Strings.YesLabel,
               Strings.NoLabel))
        {
            return;
        }

        try
        {
            IsBusy = true;
            var command = new DeletePaymentByIdCommand(payment.Id);

            if(payment.IsRecurring)
            {
                command.DeleteRecurringPayment = await dialogService.ShowConfirmMessageAsync(
                    Strings.DeleteRecurringPaymentTitle,
                    Strings.DeleteRecurringPaymentMessage);
            }

            await mediator.Send(command);
            Messenger.Send(new ReloadMessage());
        }
        catch(Exception ex)
        {
            logManager.Error(ex, "Error during deleting payment.");
            await dialogService.ShowMessageAsync(Strings.GeneralErrorTitle, Strings.UnknownErrorMessage);
        }
        finally
        {
            IsBusy = false;
        }
    }
}