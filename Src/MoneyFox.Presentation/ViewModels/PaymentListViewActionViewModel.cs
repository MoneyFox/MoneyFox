using System;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels.Interfaces;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Messages;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.ViewModels;
using IDialogService = MoneyFox.ServiceLayer.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    /// <inheritdoc cref="IPaymentListViewActionViewModel"/> />
    public class PaymentListViewActionViewModel : BaseViewModel, IPaymentListViewActionViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly ISettingsFacade settingsFacade;
        private readonly IDialogService dialogService;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly INavigationService navigationService;
        private readonly IMessenger messenger;
        private readonly int accountId;
        private bool isClearedFilterActive;
        private bool isRecurringFilterActive;
        private DateTime timeRangeStart = DateTime.Now.AddMonths(-2);
        private DateTime timeRangeEnd = DateTime.Now.AddMonths(6);
        private bool isTransferAvailable;
        private bool isAddIncomeAvailable;
        private bool isAddExpenseAvailable;

        /// <summary>
        ///     Constructor
        /// </summary>
        public PaymentListViewActionViewModel(ICrudServicesAsync crudServices,
                                              ISettingsFacade settingsFacade,
                                              IDialogService dialogService,
                                              IBalanceViewModel balanceViewModel,
                                              IMessenger messenger,
                                              int accountId,
                                              INavigationService navigationService)
        {
            this.crudServices = crudServices;
            this.settingsFacade = settingsFacade;
            this.dialogService = dialogService;
            this.balanceViewModel = balanceViewModel;
            this.navigationService = navigationService;
            this.messenger = messenger;
            this.accountId = accountId;

            var accountCount = crudServices.ReadManyNoTracked<AccountViewModel>().Count();

            IsTransferAvailable = accountCount >= 2;
            IsAddIncomeAvailable = accountCount >= 1;
            IsAddExpenseAvailable = accountCount >= 1;
        }

        /// <inheritdoc />
        public RelayCommand GoToAddIncomeCommand =>
            new RelayCommand( () => navigationService.NavigateTo(ViewModelLocator.AddPayment, new ModifyPaymentParameter(PaymentType.Income)));

        /// <inheritdoc />
        public RelayCommand GoToAddExpenseCommand =>
            new RelayCommand(() => navigationService.NavigateTo(ViewModelLocator.AddPayment, new ModifyPaymentParameter(PaymentType.Expense)));
        
        /// <inheritdoc />
        public RelayCommand GoToAddTransferCommand =>
            new RelayCommand(() => navigationService.NavigateTo(ViewModelLocator.AddPayment, new ModifyPaymentParameter(PaymentType.Transfer)));

        /// <inheritdoc />
        public RelayCommand DeleteAccountCommand => new RelayCommand(DeleteAccount);


        /// <summary>
        ///     Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        public bool IsTransferAvailable
        {
            get => isTransferAvailable;
            set
            {
                if (isTransferAvailable == value) return;
                isTransferAvailable = value;
                RaisePropertyChanged();
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
                if (isAddIncomeAvailable == value) return;
                isAddIncomeAvailable = value;
                RaisePropertyChanged();
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
                if(IsAddExpenseAvailable == value) return;
                isAddExpenseAvailable = value;
                RaisePropertyChanged();
            }
        }

        /// <inheritdoc />
        public bool IsClearedFilterActive
        {
            get => isClearedFilterActive;
            set
            {
                if (isClearedFilterActive == value) return;
                isClearedFilterActive = value;
                RaisePropertyChanged();
                UpdateList();
            }
        }

        /// <inheritdoc />
        public bool IsRecurringFilterActive
        {
            get => isRecurringFilterActive;
            set
            {
                if (isRecurringFilterActive == value) return;
                isRecurringFilterActive = value;
                RaisePropertyChanged();
                UpdateList();
            }
        }

        /// <inheritdoc />
        public DateTime TimeRangeStart
        {
            get => timeRangeStart;
            set
            {
                if (timeRangeStart == value) return;
                timeRangeStart = value;
                RaisePropertyChanged();
                UpdateList();
            }
        }

        /// <inheritdoc />
        public DateTime TimeRangeEnd
        {
            get => timeRangeEnd;
            set
            {
                if (timeRangeEnd == value) return;
                timeRangeEnd = value;
                RaisePropertyChanged();
                UpdateList();
            }
        }

        private async Task DeleteAccount()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await crudServices.DeleteAndSaveAsync<AccountViewModel>(accountId);
                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                navigationService.GoBack();
            }
            balanceViewModel.UpdateBalanceCommand.Execute();
        }

        private void UpdateList()
        {
            messenger.Send(new PaymentListFilterChangedMessage(this)
            {
                IsClearedFilterActive = IsClearedFilterActive,
                IsRecurringFilterActive = IsRecurringFilterActive,
                TimeRangeStart = this.TimeRangeStart,
                TimeRangeEnd = this.TimeRangeEnd
            });
        }
    }
}
