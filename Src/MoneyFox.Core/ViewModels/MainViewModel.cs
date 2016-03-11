using System;
using System.Globalization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Core.SettingAccess;
using MoneyFox.Foundation.Constants;
using MoneyManager.Foundation;

namespace MoneyFox.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        /// <summary>
        ///     Returns the timestamp when the last change was made to the database
        /// </summary>
        public string TimeStampDbUpdate => Settings.LastDatabaseUpdate.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///     Prepare everything and navigate to the add payment view
        /// </summary>
        public RelayCommand<string> GoToAddPaymentCommand => new RelayCommand<string>(GoToAddPayment);

        /// <summary>
        ///     Prepare everything and navigate to the add account view
        /// </summary>
        public RelayCommand GoToAddAccountCommand => new RelayCommand(GoToAddAccount);

        /// <summary>
        ///     Navigates to the recurring payment overview.
        /// </summary>
        public RelayCommand GoToRecurringPaymentListCommand => new RelayCommand(GoToRecurringPaymentList);

        private void GoToAddPayment(string typeString)
        {
            navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW,
                Enum.Parse(typeof (PaymentType), typeString));
        }

        private void GoToAddAccount()
        {
            navigationService.NavigateTo(NavigationConstants.MODIFY_ACCOUNT_VIEW);
        }

        private void GoToRecurringPaymentList()
        {
            navigationService.NavigateTo(NavigationConstants.RECURRING_PAYMENT_LIST_VIEW);
        }
    }
}