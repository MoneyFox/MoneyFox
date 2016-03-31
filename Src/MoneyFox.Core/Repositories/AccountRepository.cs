using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Model;
using MoneyFox.Core.Resources;
using PropertyChanged;

namespace MoneyFox.Core.Repositories
{
    [ImplementPropertyChanged]
    public class AccountRepository : IAccountRepository
    {
        private readonly IGenericDataRepository<Account> dataAccess;
        private ObservableCollection<Account> data;

        /// <summary>
        ///     Creates a AccountRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced a <see cref="IGenericDataRepository{T}" /> type of <see cref="Account" /></param>
        public AccountRepository(IGenericDataRepository<Account> dataAccess)
        {
            this.dataAccess = dataAccess;

            Data = new ObservableCollection<Account>();
            Load();
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

            if (account.Id == 0)
            {
                data.Add(account);
                dataAccess.Add(account);
            }
            else
            {
                dataAccess.Update(account);
            }
        }

        /// <summary>
        ///     Deletes the passed account and removes it from cache
        /// </summary>
        /// <param name="accountToDelete">accountToDelete to delete</param>
        public void Delete(Account accountToDelete)
        {
            data.Remove(accountToDelete);
            dataAccess.Delete(accountToDelete);
        }

        /// <summary>
        ///     Loads all accounts from the database to the data collection
        /// </summary>
        public void Load()
        {
            Data.Clear();

            foreach (var account in dataAccess.GetList())
            {
                Data.Add(account);
            }
        }

        /// <summary>
        ///     Adds the PaymentViewModel amount from the selected account
        /// </summary>
        /// <param name="paymentVm">PaymentViewModel to add the account from.</param>
        public void AddPaymentAmount(PaymentViewModel paymentVm)
        {
            if (!paymentVm.IsCleared)
            {
                return;
            }

            PrehandleAddIfTransfer(paymentVm);

            Func<double, double> amountFunc = x =>
                paymentVm.Type == PaymentType.Income
                    ? x
                    : -x;

            if (paymentVm.ChargedAccount == null)
            {
                paymentVm.ChargedAccount = data.FirstOrDefault(x => x.Id == paymentVm.ChargedAccount.Id);
            }

            HandlePaymentViewModelAmount(paymentVm, amountFunc, GetChargedAccountFunc(paymentVm.ChargedAccount));
        }

        /// <summary>
        ///     Removes the PaymentViewModel Amount from the charged account of this PaymentViewModel
        /// </summary>
        /// <param name="paymentVm">PaymentViewModel to remove the account from.</param>
        public void RemovePaymentAmount(PaymentViewModel paymentVm)
        {
            RemovePaymentAmount(paymentVm, paymentVm.ChargedAccount);
        }

        /// <summary>
        ///     Removes the PaymentViewModel Amount from the selected account
        /// </summary>
        /// <param name="paymentVm">PaymentViewModel to remove.</param>
        /// <param name="account">Account to remove the amount from.</param>
        public void RemovePaymentAmount(PaymentViewModel paymentVm, Account account)
        {
            if (!paymentVm.IsCleared)
            {
                return;
            }

            PrehandleRemoveIfTransfer(paymentVm);

            Func<double, double> amountFunc = x =>
                paymentVm.Type == PaymentType.Income
                    ? -x
                    : x;

            HandlePaymentViewModelAmount(paymentVm, amountFunc, GetChargedAccountFunc(account));
        }

        private void PrehandleRemoveIfTransfer(PaymentViewModel paymentVm)
        {
            if (paymentVm.Type ==  PaymentType.Transfer)
            {
                Func<double, double> amountFunc = x => -x;
                HandlePaymentViewModelAmount(paymentVm, amountFunc, GetTargetAccountFunc());
            }
        }

        private void HandlePaymentViewModelAmount(PaymentViewModel paymentVm,
            Func<double, double> amountFunc,
            Func<PaymentViewModel, Account> getAccountFunc)
        {
            var account = getAccountFunc(paymentVm);
            if (account == null)
            {
                return;
            }

            account.CurrentBalance += amountFunc(paymentVm.Amount);
            Save(account);
        }

        private void PrehandleAddIfTransfer(PaymentViewModel paymentVm)
        {
            if (paymentVm.Type == PaymentType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                HandlePaymentViewModelAmount(paymentVm, amountFunc, GetTargetAccountFunc());
            }
        }

        private Func<PaymentViewModel, Account> GetTargetAccountFunc()
        {
            //TODO: Check if needs refactoring
            Func<PaymentViewModel, Account> targetAccountFunc =
                trans => Data.FirstOrDefault(x => x.Id == trans.TargetAccount.Id);
            return targetAccountFunc;
        }

        private Func<PaymentViewModel, Account> GetChargedAccountFunc(Account account)
        {
            Func<PaymentViewModel, Account> accountFunc =
                trans => Data.FirstOrDefault(x => x.Id == account.Id);
            return accountFunc;
        }
    }
}