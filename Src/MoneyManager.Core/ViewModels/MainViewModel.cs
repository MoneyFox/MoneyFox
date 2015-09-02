using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ModifyAccountViewModel modifyAccountViewModel;
        private readonly ModifyTransactionViewModel modifyTransactionViewModel;

        /// <summary>
        ///     Creates an MainViewModel object.
        /// </summary>
        public MainViewModel(ModifyAccountViewModel modifyAccountViewModel, ModifyTransactionViewModel modifyTransactionViewModel)
        {
            this.modifyAccountViewModel = modifyAccountViewModel;
            this.modifyTransactionViewModel = modifyTransactionViewModel;

            GoToAddTransactionCommand = new MvxCommand<string>(GoToAddTransaction);
            GoToAddAccountCommand = new MvxCommand(GoToAddAccount);
            GoToAboutCommand = new MvxCommand(GoToAbout);
        }

        /// <summary>
        ///     Prepare everything and navigate to AddTransaction view
        /// </summary>
        public MvxCommand<string> GoToAddTransactionCommand { get; private set; }

        /// <summary>
        ///     Navigates to the About view
        /// </summary>
        public MvxCommand GoToAboutCommand { get; private set; }

        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        public MvxCommand GoToAddAccountCommand { get; private set; }

        private void GoToAddTransaction(string transactionType)
        {
            modifyTransactionViewModel.IsEdit = false;
            ShowViewModel<ModifyTransactionViewModel>();
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