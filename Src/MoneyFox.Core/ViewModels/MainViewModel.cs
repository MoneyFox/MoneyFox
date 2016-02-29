using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyManager.Core.Helpers;

namespace MoneyManager.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        ///     Returns the timestamp when the last change was made to the database
        /// </summary>
        public string TimeStampDbUpdate => Settings.LastDatabaseUpdate.ToString();

        /// <summary>
        ///     Prepare everything and navigate to the add payment view
        /// </summary>
        public RelayCommand<string> GoToAddPaymentCommand => new RelayCommand<string>(GoToAddPayment);

        /// <summary>
        ///     Navigates to the About view
        /// </summary>
        public RelayCommand GoToAboutCommand => new RelayCommand(GoToAbout);

        /// <summary>
        ///     Prepare everything and navigate to the add account view
        /// </summary>
        public RelayCommand GoToAddAccountCommand => new RelayCommand(GoToAddAccount);

        /// <summary>
        ///     Navigates to the recurring payment overview.
        /// </summary>
        public RelayCommand GoToRecurringPaymentListCommand => new RelayCommand(GoToRecurringPaymentList);

        private void GoToAddPayment(string paymentType)
        {
            ShowViewModel<ModifyPaymentViewModel>(new {typeString = paymentType});
        }

        private void GoToAddAccount()
        {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = false});
        }

        private void GoToRecurringPaymentList()
        {
            ShowViewModel<RecurringPaymentListViewModel>();
        }
    }
}