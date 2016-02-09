using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyManager.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IBalanceViewModel balanceViewModel;
        private readonly ModifyAccountViewModel modifyAccountViewModel;
        private readonly ModifyPaymentViewModel modifyPaymentViewModel;

        /// <summary>
        ///     Creates an MainViewModel object.
        /// </summary>
        public MainViewModel(ModifyAccountViewModel modifyAccountViewModel,
            ModifyPaymentViewModel modifyPaymentViewModel,
            IBalanceViewModel balanceViewModel)
        {
            this.modifyAccountViewModel = modifyAccountViewModel;
            this.modifyPaymentViewModel = modifyPaymentViewModel;
            this.balanceViewModel = balanceViewModel;
        }

        /// <summary>
        ///     Prepares the MainView
        /// </summary>
        public MvxCommand LoadedCommand => new MvxCommand(Loaded);

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

        private void Loaded()
        {
            balanceViewModel.UpdateBalance();
        }

        private void GoToAddPayment(string paymentType)
        {
            ShowViewModel<ModifyPaymentViewModel>(new {typeString = paymentType});
        }

        private void GoToAddAccount()
        {
            modifyAccountViewModel.IsEdit = false;
            modifyAccountViewModel.SelectedAccount = new Account();

            ShowViewModel<ModifyAccountViewModel>();
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
            ShowViewModel<BalanceViewModel>();
        }
    }
}