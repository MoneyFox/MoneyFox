using System;
using System.Linq;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Manager
{
    public class TransactionManager
    {
        private readonly IRepository<Account> accountRepository;
        private readonly AddTransactionViewModel addTransactionViewModel;
        private readonly SettingDataAccess settings;

        /// <summary>
        ///     Creates an TransactionManager object.
        /// </summary>
        /// <param name="addTransactionViewModel">Instance of <see cref="AddTransactionViewModel"/></param>
        /// <param name="accountRepository">Instance of <see cref="IRepository{T}"/></param>
        /// <param name="settings">Instance of <see cref="SettingDataAccess"/></param>
        public TransactionManager(AddTransactionViewModel addTransactionViewModel,
            IRepository<Account> accountRepository,
            SettingDataAccess settings)
        {
            this.addTransactionViewModel = addTransactionViewModel;
            this.accountRepository = accountRepository;
            this.settings = settings;
        }

        /// <summary>
        ///     Prepares everything to creat a new transaction.
        /// </summary>
        /// <param name="transactionType">Type of the transaction who shall be prepared.</param>
        public void PrepareCreation(string transactionType)
        {
            var type = (TransactionType) Enum.Parse(typeof (TransactionType), transactionType);

            addTransactionViewModel.IsEdit = false;
            addTransactionViewModel.IsEndless = true;

            //TODO: Find a way to properly refresh this list
            //addTransactionViewModel.RefreshRealtedList = refreshRelatedList;
            addTransactionViewModel.IsTransfer = type == TransactionType.Transfer;

            //TODO: Move this to the add Transaction ViewModel
            //set default that the selection is properly
            SetDefaultTransaction(type);
            SetDefaultAccount();
        }

        private void SetDefaultTransaction(TransactionType transactionType)
        {
            addTransactionViewModel.SelectedTransaction = new FinancialTransaction
            {
                Type = (int) transactionType,
                IsExchangeModeActive = false
                //Todo: refactor this / move this to own class
                //Currency = new GeographicRegion().CurrenciesInUse.First()
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