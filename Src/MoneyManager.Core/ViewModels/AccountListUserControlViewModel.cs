using System.Collections.ObjectModel;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.ViewModels
{
    //TODO Rename this to AccountListViewModel
    public class AccountListUserControlViewModel : BaseViewModel
    {
        private readonly IRepository<Account> accountRepository;

        public AccountListUserControlViewModel(IRepository<Account> accountRepository)
        {
            this.accountRepository = accountRepository;
            OpenOverviewCommand = new MvxCommand<Account>(GoToTransactionOverView);
            DeleteAccountCommand = new MvxCommand<Account>(Delete);
        }

        public ObservableCollection<Account> AllAccounts
        {
            get { return accountRepository.Data; }
            set { accountRepository.Data = value; }
        }

        public MvxCommand<Account> OpenOverviewCommand { get; set; }

        public MvxCommand<Account> DeleteAccountCommand { get; set; }
        
        private void GoToTransactionOverView(Account selectedAccount)
        {
            accountRepository.Selected = selectedAccount;
            ShowViewModel<TransactionListViewModel>();
        }

        private void Delete(Account item)
        {
            accountRepository.Delete(item);
        }
    }
}