using System;
using System.Globalization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Core.Constants;
using MoneyFox.Core.SettingAccess;

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
        ///     Prepare everything and navigate to the add PaymentViewModel view
        /// </summary>
        public RelayCommand<string> GoToAddPaymentViewModelCommand => new RelayCommand<string>(GoToAddPaymentViewModel);

        /// <summary>
        ///     Prepare everything and navigate to the add account view
        /// </summary>
        public RelayCommand GoToAddAccountCommand => new RelayCommand(GoToAddAccount);

        /// <summary>
        ///     Navigates to the recurring PaymentViewModel overview.
        /// </summary>
        public RelayCommand GoToRecurringPaymentViewModelListCommand => new RelayCommand(GoToRecurringPaymentViewModelList);

        private void GoToAddPaymentViewModel(string typeString)
        {
            navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW,
                Enum.Parse(typeof (PaymentType), typeString));
        }

        private void GoToAddAccount()
        {
            navigationService.NavigateTo(NavigationConstants.MODIFY_ACCOUNT_VIEW);
        }

        private void GoToRecurringPaymentViewModelList()
        {
            navigationService.NavigateTo(NavigationConstants.RECURRING_PAYMENT_LIST_VIEW);
        }
    }
}