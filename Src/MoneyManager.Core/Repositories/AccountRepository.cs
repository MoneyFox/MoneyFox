using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.Repositories
{
    [ImplementPropertyChanged]
    public class AccountRepository : IAccountRepository
    {
        private readonly IDataAccess<Account> dataAccess;
        private ObservableCollection<Account> data;

        /// <summary>
        ///     Creates a AccountRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced account data Access</param>
        public AccountRepository(IDataAccess<Account> dataAccess)
        {
            this.dataAccess = dataAccess;
            data = new ObservableCollection<Account>(this.dataAccess.LoadList());
        }

        /// <summary>
        ///     Cached account data
        /// </summary>
        public ObservableCollection<Account> Data
        {
            get { return data ?? (data = new ObservableCollection<Account>(dataAccess.LoadList())); }
            set
            {
                if (data == null)
                {
                    data = new ObservableCollection<Account>(dataAccess.LoadList());
                }
                if (Equals(data, value))
                {
                    return;
                }
                data = value;
            }
        }

        public Account Selected { get; set; }

        /// <summary>
        ///     SaveItem a new item or update an existin one.
        /// </summary>
        /// <param name="item">item to save</param>
        public void Save(Account item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                item.Name = Strings.NoNamePlaceholderLabel;
            }

            if (item.Id == 0)
            {
                data.Add(item);
            }
            dataAccess.SaveItem(item);
        }

        /// <summary>
        ///     Deletes the passed item and removes the item from cache
        /// </summary>
        /// <param name="item">item to delete</param>
        public void Delete(Account item)
        {
            data.Remove(item);
            dataAccess.DeleteItem(item);
        }

        /// <summary>
        ///     Loads all accounts from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Account, bool>> filter = null)
        {
            Data = new ObservableCollection<Account>(dataAccess.LoadList(filter));
        }

        /// <summary>
        ///     Adds the transaction Amount from the selected account
        /// </summary>
        /// <param name="transaction">Transaction to add the account from.</param>
        public void AddTransactionAmount(Payment transaction)
        {
            if (!transaction.IsCleared) return;

            PrehandleAddIfTransfer(transaction);

            Func<double, double> amountFunc = x =>
                transaction.Type == (int) TransactionType.Income
                    ? x
                    : -x;

            HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
        }

        /// <summary>
        ///     Removes the transaction Amount from the selected account
        /// </summary>
        /// <param name="transaction">Transaction to remove the account from.</param>
        public void RemoveTransactionAmount(Payment transaction)
        {
            if (!transaction.IsCleared) return;

            PrehandleRemoveIfTransfer(transaction);

            Func<double, double> amountFunc = x =>
                transaction.Type == (int) TransactionType.Income
                    ? -x
                    : x;

            HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
        }

        private void PrehandleRemoveIfTransfer(Payment transaction)
        {
            if (transaction.Type == (int) TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => -x;
                HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private void HandleTransactionAmount(Payment transaction,
            Func<double, double> amountFunc,
            Func<Payment, Account> getAccountFunc)
        {
            var account = getAccountFunc(transaction);
            if (account == null)
            {
                return;
            }

            account.CurrentBalance += amountFunc(transaction.Amount);
            Save(account);
        }

        private void PrehandleAddIfTransfer(Payment transaction)
        {
            if (transaction.Type == (int) TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private Func<Payment, Account> GetTargetAccountFunc()
        {
            Func<Payment, Account> targetAccountFunc =
                trans => Data.FirstOrDefault(x => x.Id == trans.TargetAccountId);
            return targetAccountFunc;
        }

        private Func<Payment, Account> GetChargedAccountFunc()
        {
            Func<Payment, Account> accountFunc =
                trans => Data.FirstOrDefault(x => x.Id == trans.ChargedAccountId);
            return accountFunc;
        }
    }
}