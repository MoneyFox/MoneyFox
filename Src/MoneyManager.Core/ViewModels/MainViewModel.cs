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
        ///     Prepare everything and navigate to AddTransaction view
        /// </summary>
        public MvxCommand<string> GoToAddTransactionCommand => new MvxCommand<string>(GoToAddTransaction);

        /// <summary>
        ///     Navigates to the About view
        /// </summary>
        public MvxCommand GoToAboutCommand => new MvxCommand(GoToAbout);

        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        public MvxCommand GoToAddAccountCommand => new MvxCommand(GoToAddAccount);

        private void Loaded()
        {
            balanceViewModel.UpdateBalance();
        }

        private void GoToAddTransaction(string transactionType)
        {
            ShowViewModel<ModifyPaymentViewModel>(new {typeString = transactionType});
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
    }
}