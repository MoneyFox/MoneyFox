using MvvmCross.Core.ViewModels;
using PropertyChanged;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Shared.ViewModels {

    [ImplementPropertyChanged]
    public class MainViewModel : BaseViewModel {

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

        private void GoToAddPayment(string paymentType) {
            ShowViewModel<ModifyPaymentViewModel>(new {typeString = paymentType});
        }

        private void GoToAddAccount() {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = false});
        }

        private void GoToAbout() {
            ShowViewModel<AboutViewModel>();
        }

        private void GoToRecurringPaymentList() {
            ShowViewModel<RecurringPaymentListViewModel>();
        }

        //Only used in Android
        public void ShowMenuAndFirstDetail() {
            ShowViewModel<MenuViewModel>();
            ShowViewModel<AccountListViewModel>();
        }
    }
}