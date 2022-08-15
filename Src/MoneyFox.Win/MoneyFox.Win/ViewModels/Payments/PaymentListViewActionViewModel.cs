namespace MoneyFox.Win.ViewModels.Payments;

using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using Core.Commands.Accounts.DeleteAccountById;
using Core.Common.Facades;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Resources;
using Interfaces;
using MediatR;
using Services;

internal sealed class PaymentListViewActionViewModel : BaseViewModel, IPaymentListViewActionViewModel
{
    private const int TRANSFER_THRESHOLD = 2;

    private readonly int accountId;
    private readonly IBalanceViewModel balanceViewModel;
    private readonly IDialogService dialogService;

    private readonly IMediator mediator;
    private readonly INavigationService navigationService;
    private readonly ISettingsFacade settingsFacade;
    private PaymentTypeFilter filteredPaymentType = PaymentTypeFilter.All;
    private bool isAddExpenseAvailable;
    private bool isAddIncomeAvailable;
    private bool isClearedFilterActive;
    private bool isGrouped;
    private bool isRecurringFilterActive;
    private bool isTransferAvailable;
    private DateTime timeRangeEnd = DateTime.Now.AddMonths(6);
    private DateTime timeRangeStart = DateTime.Now.AddMonths(-2);

    /// <summary>
    ///     Constructor
    /// </summary>
    public PaymentListViewActionViewModel(
        int accountId,
        IMediator mediator,
        ISettingsFacade settingsFacade,
        IDialogService dialogService,
        IBalanceViewModel balanceViewModel,
        INavigationService navigationService)
    {
        this.accountId = accountId;
        this.mediator = mediator;
        this.settingsFacade = settingsFacade;
        this.dialogService = dialogService;
        this.balanceViewModel = balanceViewModel;
        this.navigationService = navigationService;
        var accountCount = mediator.Send(new GetAccountCountQuery()).Result;
        IsTransferAvailable = accountCount >= TRANSFER_THRESHOLD;
        IsAddIncomeAvailable = accountCount >= 1;
        IsAddExpenseAvailable = accountCount >= 1;
    }

    /// <inheritdoc />
    public RelayCommand GoToAddIncomeCommand => new(() => navigationService.Navigate<AddPaymentViewModel>(PaymentType.Income));

    /// <inheritdoc />
    public RelayCommand GoToAddExpenseCommand => new(() => navigationService.Navigate<AddPaymentViewModel>(PaymentType.Expense));

    /// <inheritdoc />
    public RelayCommand GoToAddTransferCommand => new(() => navigationService.Navigate<AddPaymentViewModel>(PaymentType.Transfer));

    /// <summary>
    ///     Indicates if the transfer option is available or if it shall be hidden.
    /// </summary>
    public bool IsTransferAvailable
    {
        get => isTransferAvailable;

        set
        {
            if (isTransferAvailable == value)
            {
                return;
            }

            isTransferAvailable = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Indicates if the button to add new income should be enabled.
    /// </summary>
    public bool IsAddIncomeAvailable
    {
        get => isAddIncomeAvailable;

        set
        {
            if (isAddIncomeAvailable == value)
            {
                return;
            }

            isAddIncomeAvailable = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Indicates if the button to add a new expense should be enabled.
    /// </summary>
    public bool IsAddExpenseAvailable
    {
        get => isAddExpenseAvailable;

        set
        {
            if (IsAddExpenseAvailable == value)
            {
                return;
            }

            isAddExpenseAvailable = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public AsyncRelayCommand DeleteAccountCommand => new(DeleteAccountAsync);

    /// <inheritdoc />
    public RelayCommand ApplyFilterCommand => new(ApplyFilter);

    /// <inheritdoc />
    public bool IsClearedFilterActive
    {
        get => isClearedFilterActive;

        set
        {
            if (isClearedFilterActive == value)
            {
                return;
            }

            isClearedFilterActive = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public bool IsRecurringFilterActive
    {
        get => isRecurringFilterActive;

        set
        {
            if (isRecurringFilterActive == value)
            {
                return;
            }

            isRecurringFilterActive = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public bool IsGrouped
    {
        get => isGrouped;

        set
        {
            if (isGrouped == value)
            {
                return;
            }

            isGrouped = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public PaymentTypeFilter FilteredPaymentType
    {
        get => filteredPaymentType;

        set
        {
            if (filteredPaymentType == value)
            {
                return;
            }

            filteredPaymentType = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public DateTime TimeRangeStart
    {
        get => timeRangeStart;

        set
        {
            if (timeRangeStart == value)
            {
                return;
            }

            timeRangeStart = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public DateTime TimeRangeEnd
    {
        get => timeRangeEnd;

        set
        {
            if (timeRangeEnd == value)
            {
                return;
            }

            timeRangeEnd = value;
            OnPropertyChanged();
        }
    }

    private async Task DeleteAccountAsync()
    {
        if (await dialogService.ShowConfirmMessageAsync(title: Strings.DeleteTitle, message: Strings.DeleteAccountConfirmationMessage))
        {
            await mediator.Send(new DeactivateAccountByIdCommand(accountId));
            settingsFacade.LastDatabaseUpdate = DateTime.Now;
            navigationService.GoBack();
        }

        await balanceViewModel.UpdateBalanceCommand.ExecuteAsync(null);
    }

    private void ApplyFilter()
    {
        Messenger.Send(
            new PaymentListFilterChangedMessage
            {
                IsClearedFilterActive = IsClearedFilterActive,
                IsRecurringFilterActive = IsRecurringFilterActive,
                TimeRangeStart = TimeRangeStart,
                TimeRangeEnd = TimeRangeEnd,
                IsGrouped = IsGrouped,
                FilteredPaymentType = FilteredPaymentType
            });
    }
}
