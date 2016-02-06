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

        public Account Selected { get; set; }

        /// <summary>
        ///     Cached account data
        /// </summary>
        public ObservableCollection<Account> Data
        {
            get { return data; }
            set
            {
                if (Equals(data, value))
                {
                    return;
                }
                data = value;
            }
        }

        /// <summary>
        ///     Save a new account or update an existing one.
        /// </summary>
        /// <param name="account">accountToDelete to save</param>
        public void Save(Account account)
        {
            if (string.IsNullOrWhiteSpace(account.Name))
            {
                account.Name = Strings.NoNamePlaceholderLabel;
            }

            dataAccess.SaveItem(account);

            if (account.Id == 0)
            {
                // Reload data
                Load();
            }
        }

        /// <summary>
        ///     Deletes the passed account and removes it from cache
        /// </summary>
        /// <param name="accountToDelete">accountToDelete to delete</param>
        public void Delete(Account accountToDelete)
        {
            data.Remove(accountToDelete);
            dataAccess.DeleteItem(accountToDelete);
        }

        /// <summary>
        ///     Loads all accounts from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Account, bool>> filter = null)
        {
            Data = new ObservableCollection<Account>();

            foreach (var account in dataAccess.LoadList(filter))
            {
                Data.Add(account);
            }
        }

        /// <summary>
        ///     Adds the payment amount from the selected account
        /// </summary>
        /// <param name="payment">Payment to add the account from.</param>
        public void AddPaymentAmount(Payment payment)
        {
            if (!payment.IsCleared) return;

            PrehandleAddIfTransfer(payment);

            Func<double, double> amountFunc = x =>
                payment.Type == (int)PaymentType.Income
                    ? x
                    : -x;

            HandlePaymentAmount(payment, amountFunc, GetChargedAccountFunc());
        }

        /// <summary>
        ///     Removes the payment Amount from the selected account
        /// </summary>
        /// <param name="payment">Payment to remove the account from.</param>
        public void RemovePaymentAmount(Payment payment)
        {
            if (!payment.IsCleared) return;

            PrehandleRemoveIfTransfer(payment);

            Func<double, double> amountFunc = x =>
                payment.Type == (int)PaymentType.Income
                    ? -x
                    : x;

            HandlePaymentAmount(payment, amountFunc, GetChargedAccountFunc());
        }

        private void PrehandleRemoveIfTransfer(Payment payment)
        {
            if (payment.Type == (int)PaymentType.Transfer)
            {
                Func<double, double> amountFunc = x => -x;
                HandlePaymentAmount(payment, amountFunc, GetTargetAccountFunc());
            }
        }

        private void HandlePaymentAmount(Payment payment,
            Func<double, double> amountFunc,
            Func<Payment, Account> getAccountFunc)
        {
            var account = getAccountFunc(payment);
            if (account == null)
            {
                return;
            }

            account.CurrentBalance += amountFunc(payment.Amount);
            Save(account);
        }

        private void PrehandleAddIfTransfer(Payment payment)
        {
            if (payment.Type == (int)PaymentType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                HandlePaymentAmount(payment, amountFunc, GetTargetAccountFunc());
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