using System;
using System.Linq;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Messages;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.ViewModels.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <inheritdoc cref="IPaymentListViewActionViewModel"/> />
    public class PaymentListViewActionViewModel : BaseNavigationViewModel, IPaymentListViewActionViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly ISettingsFacade settingsFacade;
        private readonly IDialogService dialogService;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly IMvxNavigationService navigationService;
        private readonly IMvxMessenger messenger;
        private readonly int accountId;
        private bool isClearedFilterActive;
        private bool isRecurringFilterActive;
        private DateTime timeRangeStart = DateTime.Now.AddMonths(-2);
        private DateTime timeRangeEnd = DateTime.Now.AddMonths(6);

        /// <summary>
        ///     Constructor
        /// </summary>
        public PaymentListViewActionViewModel(ICrudServicesAsync crudServices,
                                              ISettingsFacade settingsFacade,
                                              IDialogService dialogService,
                                              IBalanceViewModel balanceViewModel,
                                              IMvxMessenger messenger,
                                              int accountId,
                                              IMvxLogProvider logProvider,
                                              IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.crudServices = crudServices;
            this.settingsFacade = settingsFacade;
            this.dialogService = dialogService;
            this.balanceViewModel = balanceViewModel;
            this.navigationService = navigationService;
            this.messenger = messenger;
            this.accountId = accountId;
        }

        #region Commands

        /// <inheritdoc />
        public MvxAsyncCommand GoToAddIncomeCommand =>
            new MvxAsyncCommand(async () => await navigationService
                                    .Navigate<AddPaymentViewModel, ModifyPaymentParameter>(
                                        new ModifyPaymentParameter(PaymentType.Income)));

        /// <inheritdoc />
        public MvxAsyncCommand GoToAddExpenseCommand =>
            new MvxAsyncCommand(async () => await navigationService
                                    .Navigate<AddPaymentViewModel, ModifyPaymentParameter>(
                                        new ModifyPaymentParameter(PaymentType.Expense)));
        
        /// <inheritdoc />
        public MvxAsyncCommand GoToAddTransferCommand =>
            new MvxAsyncCommand(async () => await navigationService
                                    .Navigate<AddPaymentViewModel, ModifyPaymentParameter>(
                                        new ModifyPaymentParameter(PaymentType.Transfer)));

        /// <inheritdoc />
        public MvxAsyncCommand DeleteAccountCommand => new MvxAsyncCommand(DeleteAccount);


        #endregion

        #region Properties

        /// <summary>
        ///     Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        public bool IsTransferAvailable => crudServices.ReadManyNoTracked<AccountViewModel>().Count() > 1;

        /// <summary>
        ///     Indicates if the button to add new income should be enabled.
        /// </summary>
        public bool IsAddIncomeAvailable => crudServices.ReadManyNoTracked<AccountViewModel>().Any();

        /// <summary>
        ///     Indicates if the button to add a new expense should be enabled.
        /// </summary>
        public bool IsAddExpenseAvailable => crudServices.ReadManyNoTracked<AccountViewModel>().Any();

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

        #endregion

        private async Task DeleteAccount()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await crudServices.DeleteAndSaveAsync<AccountViewModel>(accountId);
                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                await navigationService.Close(this);
            }
            balanceViewModel.UpdateBalanceCommand.Execute();
        }

        private void UpdateList()
        {
            messenger.Publish(new PaymentListFilterChangedMessage(this)
            {
                IsClearedFilterActive = IsClearedFilterActive,
                IsRecurringFilterActive = IsRecurringFilterActive,
                TimeRangeStart = this.TimeRangeStart,
                TimeRangeEnd = this.TimeRangeEnd
            });
        }
    }
}
