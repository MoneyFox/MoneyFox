using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Commands.DeleteAccountById;
using MoneyFox.Application.Accounts.Queries.GetAccountCount;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Payments
{
    /// <inheritdoc cref="IPaymentListViewActionViewModel"/>
    /// />
    public class PaymentListViewActionViewModel : ViewModelBase, IPaymentListViewActionViewModel
    {
        private const int TRANSFER_THRESHOLD = 2;

        private readonly IMediator mediator;
        private readonly ISettingsFacade settingsFacade;
        private readonly IDialogService dialogService;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly NavigationService navigationService;

        private readonly int accountId;
        private bool isClearedFilterActive;
        private bool isRecurringFilterActive;
        private bool isGrouped;
        private DateTime timeRangeStart = DateTime.Now.AddMonths(-2);
        private DateTime timeRangeEnd = DateTime.Now.AddMonths(6);
        private bool isTransferAvailable;
        private bool isAddIncomeAvailable;
        private bool isAddExpenseAvailable;

        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentListViewActionViewModel(int accountId,
                                              IMediator mediator,
                                              ISettingsFacade settingsFacade,
                                              IDialogService dialogService,
                                              IBalanceViewModel balanceViewModel,
                                              NavigationService navigationService)
        {
            this.accountId = accountId;

            this.mediator = mediator;
            this.settingsFacade = settingsFacade;
            this.dialogService = dialogService;
            this.balanceViewModel = balanceViewModel;
            this.navigationService = navigationService;

#pragma warning disable S4462 // Calls to "async" methods should not be blocking
            int accountCount = mediator.Send(new GetAccountCountQuery()).Result;
#pragma warning restore S4462 // Calls to "async" methods should not be blocking

            IsTransferAvailable = accountCount >= TRANSFER_THRESHOLD;
            IsAddIncomeAvailable = accountCount >= 1;
            IsAddExpenseAvailable = accountCount >= 1;
        }

        /// <inheritdoc/>
        public RelayCommand GoToAddIncomeCommand
                            => new RelayCommand(() => navigationService.Navigate<AddPaymentViewModel>(PaymentType.Income));

        /// <inheritdoc/>
        public RelayCommand GoToAddExpenseCommand
                            => new RelayCommand(() => navigationService.Navigate<AddPaymentViewModel>(PaymentType.Expense));

        /// <inheritdoc/>
        public RelayCommand GoToAddTransferCommand
                            => new RelayCommand(() => navigationService.Navigate<AddPaymentViewModel>(PaymentType.Transfer));

        /// <inheritdoc/>
        public AsyncCommand DeleteAccountCommand => new AsyncCommand(DeleteAccountAsync);

        /// <inheritdoc/>
        public RelayCommand ApplyFilterCommand => new RelayCommand(ApplyFilter);

        /// <summary>
        /// Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        public bool IsTransferAvailable
        {
            get => isTransferAvailable;
            set
            {
                if(isTransferAvailable == value)
                {
                    return;
                }

                isTransferAvailable = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Indicates if the button to add new income should be enabled.
        /// </summary>
        public bool IsAddIncomeAvailable
        {
            get => isAddIncomeAvailable;
            set
            {
                if(isAddIncomeAvailable == value)
                {
                    return;
                }

                isAddIncomeAvailable = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Indicates if the button to add a new expense should be enabled.
        /// </summary>
        public bool IsAddExpenseAvailable
        {
            get => isAddExpenseAvailable;
            set
            {
                if(IsAddExpenseAvailable == value)
                {
                    return;
                }

                isAddExpenseAvailable = value;
                RaisePropertyChanged();
            }
        }

        /// <inheritdoc/>
        public bool IsClearedFilterActive
        {
            get => isClearedFilterActive;
            set
            {
                if(isClearedFilterActive == value)
                {
                    return;
                }

                isClearedFilterActive = value;
                RaisePropertyChanged();
            }
        }

        /// <inheritdoc/>
        public bool IsRecurringFilterActive
        {
            get => isRecurringFilterActive;
            set
            {
                if(isRecurringFilterActive == value)
                {
                    return;
                }

                isRecurringFilterActive = value;
                RaisePropertyChanged();
            }
        }

        /// <inheritdoc/>
        public bool IsGrouped
        {
            get => isGrouped;
            set
            {
                if(isGrouped == value)
                {
                    return;
                }

                isGrouped = value;
                RaisePropertyChanged();
            }
        }

        /// <inheritdoc/>
        public DateTime TimeRangeStart
        {
            get => timeRangeStart;
            set
            {
                if(timeRangeStart == value)
                {
                    return;
                }

                timeRangeStart = value;
                RaisePropertyChanged();
            }
        }

        /// <inheritdoc/>
        public DateTime TimeRangeEnd
        {
            get => timeRangeEnd;
            set
            {
                if(timeRangeEnd == value)
                {
                    return;
                }

                timeRangeEnd = value;
                RaisePropertyChanged();
            }
        }

        private async Task DeleteAccountAsync()
        {
            if(await dialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await mediator.Send(new DeactivateAccountByIdCommand(accountId));
                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                navigationService.GoBack();
            }

            await balanceViewModel.UpdateBalanceCommand.ExecuteAsync();
        }

        private void ApplyFilter()
        {
            MessengerInstance.Send(new PaymentListFilterChangedMessage
            {
                IsClearedFilterActive = IsClearedFilterActive,
                IsRecurringFilterActive = IsRecurringFilterActive,
                TimeRangeStart = TimeRangeStart,
                TimeRangeEnd = TimeRangeEnd,
                IsGrouped = IsGrouped
            });
        }
    }
}
