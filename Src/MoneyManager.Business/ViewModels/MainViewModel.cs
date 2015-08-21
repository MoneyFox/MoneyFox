using System;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        private readonly AddTransactionViewModel addTransactionViewModel;
        private readonly IRepository<Account> accountRepository;
        private readonly SettingDataAccess settings;
        private readonly INavigationService navigationService;

        public RelayCommand<string> GoToAddTransactionCommand { get; private set; }
        public RelayCommand GoToAddAccountCommand { get; private set; }
        
        public MainViewModel(AddTransactionViewModel addTransactionViewModel, IRepository<Account> accountRepository, SettingDataAccess settings, INavigationService navigationService)
        {
            this.addTransactionViewModel = addTransactionViewModel;
            this.navigationService = navigationService;
            this.settings = settings;
            this.accountRepository = accountRepository;

            GoToAddTransactionCommand = new RelayCommand<string>(GoToAddTransaction);
            GoToAddAccountCommand = new RelayCommand(() => navigationService.NavigateTo("AddAccountView"));
        }
        
        private void GoToAddTransaction(string transactionType)
        {
            var type = (TransactionType) Enum.Parse(typeof (TransactionType), transactionType);

            ServiceLocator.Current.GetInstance<CategoryListViewModel>().IsSettingCall = false;
            addTransactionViewModel.IsEdit = false;
            addTransactionViewModel.IsEndless = true;

            //TODO: Find a way to properly refresh this list
            //addTransactionViewModel.RefreshRealtedList = refreshRelatedList;
            addTransactionViewModel.IsTransfer = type == TransactionType.Transfer;

            //TODO: Move this to the add Transaction ViewModel
            //set default that the selection is properly
            SetDefaultTransaction(type);
            SetDefaultAccount();

            navigationService.NavigateTo("AddTransactionView");
        }

        private void SetDefaultTransaction(TransactionType transactionType)
        {
            addTransactionViewModel.SelectedTransaction = new FinancialTransaction
            {
                Type = (int)transactionType,
                IsExchangeModeActive = false,
                Currency = ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency
            };
        }

        private void SetDefaultAccount()
        {
            if (accountRepository.Data.Any())
            {
                addTransactionViewModel.SelectedTransaction.ChargedAccount = accountRepository.Data.First();
            }

            if (accountRepository.Data.Any() && settings.DefaultAccount != -1)
            {
                addTransactionViewModel.SelectedTransaction.ChargedAccount =
                    accountRepository.Data.FirstOrDefault(x => x.Id == settings.DefaultAccount);
            }

            if (accountRepository.Selected != null)
            {
                addTransactionViewModel.SelectedTransaction.ChargedAccount = accountRepository.Selected;
            }
        }
    }
}
