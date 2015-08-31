using System;
using System.Collections.ObjectModel;
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
        private readonly ModifyTransactionViewModel modifyTransactionViewModel;
        private readonly SettingDataAccess settings;

        /// <summary>
        ///     Creates an TransactionManager object.
        /// </summary>
        /// <param name="modifyTransactionViewModel">Instance of <see cref="ModifyTransactionViewModel" /></param>
        /// <param name="accountRepository">Instance of <see cref="IRepository{T}" /></param>
        /// <param name="settings">Instance of <see cref="SettingDataAccess" /></param>
        public TransactionManager(ModifyTransactionViewModel modifyTransactionViewModel,
            IRepository<Account> accountRepository,
            SettingDataAccess settings)
        {
            this.modifyTransactionViewModel = modifyTransactionViewModel;
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

            modifyTransactionViewModel.IsEdit = false;
            modifyTransactionViewModel.IsEndless = true;

            //TODO: Find a way to properly refresh this list
            //ModifyTransactionViewModel.RefreshRealtedList = refreshRelatedList;
            modifyTransactionViewModel.IsTransfer = type == TransactionType.Transfer;

            //TODO: Move this to the add Transaction ViewModel
            //set default that the selection is properly
            SetDefaultTransaction(type);
            SetDefaultAccount();
        }

        private void SetDefaultTransaction(TransactionType transactionType)
        {
            modifyTransactionViewModel.SelectedTransaction = new FinancialTransaction
            {
                Type = (int) transactionType
            };
        }

        private void SetDefaultAccount()
        {
            if (accountRepository.Data == null)
            {
                accountRepository.Data = new ObservableCollection<Account>();
            }

            if (accountRepository.Data.Any())
            {
                modifyTransactionViewModel.SelectedTransaction.ChargedAccount = accountRepository.Data.First();
            }

            if (accountRepository.Data.Any() && settings.DefaultAccount != -1)
            {
                modifyTransactionViewModel.SelectedTransaction.ChargedAccount =
                    accountRepository.Data.FirstOrDefault(x => x.Id == settings.DefaultAccount);
            }

            if (accountRepository.Selected != null)
            {
                modifyTransactionViewModel.SelectedTransaction.ChargedAccount = accountRepository.Selected;
            }
        }
    }
}