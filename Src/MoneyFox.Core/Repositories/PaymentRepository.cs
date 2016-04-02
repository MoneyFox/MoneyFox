using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Core.DatabaseModels;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.ViewModels.Models;
using PropertyChanged;

namespace MoneyFox.Core.Repositories
{
    [ImplementPropertyChanged]
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IAccountRepository accountRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly IGenericDataRepository<Payment> dataAccess;
        private readonly IGenericDataRepository<RecurringPayment> recurringDataAccess;
        private ObservableCollection<PaymentViewModel> data;

        /// <summary>
        ///     Creates a PaymentRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced <see cref="IGenericDataRepository{T}" /> for <see cref="Payment" /></param>
        /// <param name="recurringDataAccess">
        ///     Instanced <see cref="IGenericDataRepository{T}" /> for <see cref="RecurringPayment" />
        /// </param>
        /// <param name="accountRepository">Instanced <see cref="IAccountRepository" /></param>
        /// <param name="categoryRepository">
        ///     Instanced <see cref="IRepository{T}" /> for <see cref="Category" />
        /// </param>
        public PaymentRepository(IGenericDataRepository<Payment> dataAccess,
            IGenericDataRepository<RecurringPayment> recurringDataAccess,
            IAccountRepository accountRepository,
            IRepository<Category> categoryRepository)
        {
            this.dataAccess = dataAccess;
            this.recurringDataAccess = recurringDataAccess;
            this.accountRepository = accountRepository;
            this.categoryRepository = categoryRepository;

            Data = new ObservableCollection<PaymentViewModel>();
            Load();
        }

        /// <summary>
        ///     Cached accountToDelete data
        /// </summary>
        public ObservableCollection<PaymentViewModel> Data
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
        ///     The currently selected Payment
        /// </summary>
        public PaymentViewModel Selected { get; set; }

        /// <summary>
        ///     Save a new payment or update an existin one.
        /// </summary>
        /// <param name="paymentVm">item to save</param>
        public void Save(PaymentViewModel paymentVm)
        {
            paymentVm.IsCleared = paymentVm.ClearPaymentNow;

            //delete recurring payment if isRecurring is no longer set.
            if (paymentVm.IsRecurring)
            {
                recurringDataAccess.Delete(paymentVm.RecurringPayment);
                paymentVm.RecurringPayment.Id = 0;
            }

            if (paymentVm.Id == 0)
            {
                data.Add(paymentVm);
                dataAccess.Add(paymentVm.GetPayment());
            }
            else
            {
                dataAccess.Update(paymentVm.GetPayment());
            }
        }

        /// <summary>
        ///     Deletes the passed payment and removes the item from cache
        /// </summary>
        /// <param name="paymentVmToDelete">Payment to delete.</param>
        public void Delete(PaymentViewModel paymentVmToDelete)
        {
            var payments = Data.Where(x => x.Id == paymentVmToDelete.Id).ToList();

            foreach (var payment in payments)
            {
                data.Remove(payment);
                dataAccess.Delete(paymentVmToDelete.GetPayment());

                // If this accountToDelete was the last finacial accountToDelete for the linked recurring accountToDelete
                // delete the db entry for the recurring accountToDelete.
                DeleteRecurringPaymentIfLastAssociated(payment);
            }
        }

        /// <summary>
        ///     Deletes the passed recurring payment
        /// </summary>
        /// <param name="paymentToDelete">Recurring payment to delete.</param>
        public void DeleteRecurring(PaymentViewModel paymentToDelete)
        {
            var payments = Data.Where(x => x.Id == paymentToDelete.Id).ToList();

            recurringDataAccess.Delete(paymentToDelete.RecurringPayment);

            foreach (var payment in payments)
            {
                payment.RecurringPayment = null;
                payment.IsRecurring = false;
                Save(payment);
            }
        }

        /// <summary>
        ///     Loads all payments from the database to the data collection
        /// </summary>
        public void Load()
        {
            Data.Clear();
            var payments = dataAccess.GetList(null, payment => payment.ChargedAccount,
                payment => payment.Category,
                payment => payment.RecurringPayment);

            foreach (var payment in payments)
            {
                Data.Add(new PaymentViewModel(payment));
            }
        }

        /// <summary>
        ///     Returns all uncleared payments up to today
        /// </summary>
        /// <returns>list of uncleared payments</returns>
        public IEnumerable<PaymentViewModel> GetUnclearedPayments()
        {
            return GetUnclearedPayments(DateTime.Today);
        }

        /// <summary>
        ///     Returns all uncleared payments up to the passed date from the database.
        /// </summary>
        /// <returns>list of uncleared payments</returns>
        public IEnumerable<PaymentViewModel> GetUnclearedPayments(DateTime date)
        {
            return Data
                .Where(x => !x.IsCleared)
                .Where(x => x.Date.Date <= date.Date)
                .ToList();
        }

        /// <summary>
        ///     returns a list with payments who are related to this account
        /// </summary>
        /// <param name="account">account to search the related</param>
        /// <returns>List of payments</returns>
        public IEnumerable<PaymentViewModel> GetRelatedPayments(Account account)
        {
            return Data.Where(x => x.ChargedAccount.Id == account.Id
                                   || x.TargetAccount.Id == account.Id)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        /// <summary>
        ///     returns a list with payments who recure in a given timeframe
        /// </summary>
        /// <returns>list of recurring payments</returns>
        public IEnumerable<PaymentViewModel> LoadRecurringList(Func<PaymentViewModel, bool> filter = null)
        {
            var list = Data
                .Where(x => x.IsRecurring && x.RecurringPayment.Id != 0)
                .Where(x => (x.RecurringPayment.IsEndless ||
                             x.RecurringPayment.EndDate >= DateTime.Now.Date)
                            && (filter == null || filter.Invoke(x)))
                .ToList();

            return list
                .Select(x => x.RecurringPayment.Id)
                .Distinct()
                .Select(id => list.Where(x => x.RecurringPayment.Id == id)
                    .OrderByDescending(x => x.Date)
                    .Last())
                .ToList();
        }

        private void DeleteRecurringPaymentIfLastAssociated(PaymentViewModel item)
        {
            if (Data.All(x => x.RecurringPayment != item.RecurringPayment))
            {
                var recurringList = recurringDataAccess.GetList(x => x.Id == item.RecurringPayment.Id).ToList();

                foreach (var recTrans in recurringList)
                {
                    recurringDataAccess.Delete(recTrans);
                }
            }
        }
    }
}