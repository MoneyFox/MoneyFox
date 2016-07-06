using MoneyFox.Shared.Interfaces;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class MainViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;

        public MainViewModel(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        /// <summary>
        ///     Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        public bool IsTransferAvailable => accountRepository.Data.Count > 1;

        /// <summary>
        ///     Indicates if the button to add new income should be enabled.
        /// </summary>
        public bool IsAddIncomeAvailable => accountRepository.Data.Count > 0;

        /// <summary>
        /// Indicates if the button to add a new expense should be enabled.
        /// </summary>
        public bool IsAddExpenseAvailable => accountRepository.Data.Count > 0;

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

        //Only used in Android
        public void ShowMenuAndFirstDetail()
        {
            ShowViewModel<MenuViewModel>();
            ShowViewModel<AccountListViewModel>();
        }
    }
}