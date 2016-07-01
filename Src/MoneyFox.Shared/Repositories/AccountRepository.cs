using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using PropertyChanged;

namespace MoneyFox.Shared.Repositories {
    [ImplementPropertyChanged]
    public class AccountRepository : IAccountRepository {
        private readonly IDataAccess<Account> dataAccess;
        private readonly INotificationService notificationService;

        private ObservableCollection<Account> data;

        /// <summary>
        ///     Creates a AccountRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced account data Access</param>
        /// <param name="notificationService">Service to notify user in case of errors.</param>
        public AccountRepository(IDataAccess<Account> dataAccess,
            INotificationService notificationService) {
            this.dataAccess = dataAccess;
            this.notificationService = notificationService;

            Data = new ObservableCollection<Account>();
            Load();
        }

        public Account Selected { get; set; }

        /// <summary>
        ///     Cached account data
        /// </summary>
        public ObservableCollection<Account> Data {
            get { return data; }
            set {
                if (Equals(data, value)) {
                    return;
                }
                data = value;
            }
        }

        /// <summary>
        ///     Save a new account or update an existing one.
        /// </summary>
        /// <param name="account">accountToDelete to save</param>
        public bool Save(Account account)
        {
            if (string.IsNullOrWhiteSpace(account.Name))
            {
                account.Name = Strings.NoNamePlaceholderLabel;
            }

            if (account.Id == 0)
            {
                data.Add(account);
            }
            if (!dataAccess.SaveItem(account))
            {
                notificationService.SendBasicNotification(Strings.ErrorTitleSave, Strings.ErrorMessageSave);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Deletes the passed account and removes it from cache
        /// </summary>
        /// <param name="accountToDelete">accountToDelete to delete</param>
        public bool Delete(Account accountToDelete) {
            data.Remove(accountToDelete);
            if (!dataAccess.DeleteItem(accountToDelete)) {
                notificationService.SendBasicNotification(Strings.ErrorTitleDelete, Strings.ErrorMessageDelete);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Loads all accounts from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Account, bool>> filter = null) {
            Data.Clear();

            foreach (var account in dataAccess.LoadList(filter)) {
                Data.Add(account);
            }
        }

        /// <summary>
        ///     Adds the payment amount from the selected account
        /// </summary>
        /// <param name="payment">Payment to add the account from.</param>
        public bool AddPaymentAmount(Payment payment) {
            if (!payment.IsCleared) {
                return false;
            }

            PrehandleAddIfTransfer(payment);

            Func<double, double> amountFunc = x =>
                payment.Type == (int) PaymentType.Income
                    ? x
                    : -x;

            if (payment.ChargedAccount == null && payment.ChargedAccountId != 0) {
                payment.ChargedAccount = data.FirstOrDefault(x => x.Id == payment.ChargedAccountId);
            }

            HandlePaymentAmount(payment, amountFunc, GetChargedAccountFunc(payment.ChargedAccount));
            return true;
        }

        /// <summary>
        ///     Removes the payment Amount from the charged account of this payment
        /// </summary>
        /// <param name="payment">Payment to remove the account from.</param>
        public bool RemovePaymentAmount(Payment payment) {
            bool succeded = RemovePaymentAmount(payment, payment.ChargedAccount);
            return succeded;
        }

        /// <summary>
        ///     Removes the payment Amount from the selected account
        /// </summary>
        /// <param name="payment">Payment to remove.</param>
        /// <param name="account">Account to remove the amount from.</param>
        public bool RemovePaymentAmount(Payment payment, Account account) {
            if (!payment.IsCleared) {
                return false;
            }

            PrehandleRemoveIfTransfer(payment);

            Func<double, double> amountFunc = x =>
                payment.Type == (int) PaymentType.Income
                    ? -x
                    : x;

            HandlePaymentAmount(payment, amountFunc, GetChargedAccountFunc(account));
            return true;
        }

        private void PrehandleRemoveIfTransfer(Payment payment) {
            if (payment.Type == (int) PaymentType.Transfer) {
                Func<double, double> amountFunc = x => -x;
                HandlePaymentAmount(payment, amountFunc, GetTargetAccountFunc());
            }
        }

        private void HandlePaymentAmount(Payment payment,
            Func<double, double> amountFunc,
            Func<Payment, Account> getAccountFunc) {
            var account = getAccountFunc(payment);
            if (account == null) {
                return;
            }

            account.CurrentBalance += amountFunc(payment.Amount);
            Save(account);
        }

        private void PrehandleAddIfTransfer(Payment payment) {
            if (payment.Type == (int) PaymentType.Transfer) {
                Func<double, double> amountFunc = x => x;
                HandlePaymentAmount(payment, amountFunc, GetTargetAccountFunc());
            }
        }

        private Func<Payment, Account> GetTargetAccountFunc() {
            Func<Payment, Account> targetAccountFunc =
                trans => Data.FirstOrDefault(x => x.Id == trans.TargetAccountId);
            return targetAccountFunc;
        }

        private Func<Payment, Account> GetChargedAccountFunc(Account account) {
            Func<Payment, Account> accountFunc =
                trans => Data.FirstOrDefault(x => x.Id == account.Id);
            return accountFunc;
        }
    }
}