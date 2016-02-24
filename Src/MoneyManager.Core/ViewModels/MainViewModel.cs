using MoneyManager.Core.Helpers;
using MvvmCross.Core.ViewModels;

namespace MoneyManager.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        /// <summary>
        ///     Returns the timestamp when the last change was made to the database
        /// </summary>
        public string TimeStampDbUpdate => Settings.LastDatabaseUpdate.ToString();

        /// <summary>
        ///     Prepare everything and navigate to the add payment view
        /// </summary>
        public MvxCommand<string> GoToAddPaymentCommand => new MvxCommand<string>(GoToAddPayment);

        /// <summary>
        ///     Navigates to the About view
        /// </summary>
        public MvxCommand GoToAboutCommand => new MvxCommand(GoToAbout);

        /// <summary>
        ///     Prepare everything and navigate to the add account view
        /// </summary>
        public MvxCommand GoToAddAccountCommand => new MvxCommand(GoToAddAccount);

        /// <summary>
        ///     Navigates to the recurring payment overview.
        /// </summary>
        public MvxCommand GoToRecurringPaymentListCommand => new MvxCommand(GoToRecurringPaymentList);

        private void GoToAddPayment(string paymentType)
        {
            ShowViewModel<ModifyPaymentViewModel>(new {typeString = paymentType});
        }

        private void GoToAddAccount()
        {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = false});
        }

        private void GoToAbout()
        {
            ShowViewModel<AboutViewModel>();
        }

        private void GoToRecurringPaymentList()
        {
            ShowViewModel<RecurringPaymentListViewModel>();
        }

        //Only used in Android so far.
        public void ShowMenuAndFirstDetail()
        {
            ShowViewModel<MenuViewModel>();
            ShowViewModel<AccountListViewModel>();
        }
    }
}