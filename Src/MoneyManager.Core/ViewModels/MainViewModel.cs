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
        public MainViewModel(ModifyAccountViewModel modifyAccountViewModel,
            ModifyTransactionViewModel modifyTransactionViewModel)
        {
            this.modifyAccountViewModel = modifyAccountViewModel;
            this.modifyTransactionViewModel = modifyTransactionViewModel;
        }

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

        private void GoToAddTransaction(string transactionType)
        {
            modifyTransactionViewModel.IsEdit = false;
            ShowViewModel<ModifyTransactionViewModel>(new {typeString = transactionType});
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