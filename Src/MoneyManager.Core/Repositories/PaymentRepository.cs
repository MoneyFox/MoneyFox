using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation.Exceptions;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.Repositories
{
    [ImplementPropertyChanged]
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDataAccess<Payment> dataAccess;
        private readonly IDataAccess<RecurringPayment> recurringDataAccess;
        private ObservableCollection<Payment> data;

        /// <summary>
        ///     Creates a paymentRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced <see cref="IDataAccess{T}" /> for <see cref="Payment" /></param>
        /// <param name="recurringDataAccess">
        ///     Instanced <see cref="IDataAccess{T}" /> for <see cref="RecurringPayment" />
        /// </param>
        public PaymentRepository(IDataAccess<Payment> dataAccess,
            IDataAccess<RecurringPayment> recurringDataAccess)
        {
            this.dataAccess = dataAccess;
            this.recurringDataAccess = recurringDataAccess;

            Load();
        }

        /// <summary>
        ///     cached paymentToDelete data
        /// </summary>
        public ObservableCollection<Payment> Data
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
        ///     The currently selected Transaction
        /// </summary>
        public Payment Selected { get; set; }

        /// <summary>
        ///     SaveItem a new item or update an existin one.
        /// </summary>
        /// <param name="item">item to save</param>
        public void Save(Payment item)
        {
            if (item.ChargedAccount == null)
            {
                throw new AccountMissingException("charged accout is missing");
            }

            item.IsCleared = item.ClearPaymentNow;

            if (item.Id == 0)
            {
                data.Add(item);
            }

            //delete recurring paymentToDelete if isRecurring is no longer set.
            if (!item.IsRecurring && item.ReccuringPaymentId != 0)
            {
                recurringDataAccess.DeleteItem(item.RecurringPayment);
                item.ReccuringPaymentId = 0;
            }

            dataAccess.SaveItem(item);
        }

        /// <summary>
        ///     Deletes the passed item and removes the item from cache
        /// </summary>
        /// <param name="paymentToDelete">item to delete</param>
        public void Delete(Payment paymentToDelete)
        {
            var payments = Data.Where(x => x.Id == paymentToDelete.Id).ToList();

            foreach (var payment in payments)
            {
                data.Remove(payment);
                dataAccess.DeleteItem(payment);

                // If this paymentToDelete was the last finacial paymentToDelete for the linked recurring paymentToDelete
                // delete the db entry for the recurring paymentToDelete.
                DeleteRecurringPaymentIfLastAssociated(payment);
            }
        }

        /// <summary>
        ///     Loads all transactions from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Payment, bool>> filter = null)
        {
            Data = new ObservableCollection<Payment>(dataAccess.LoadList(filter));
        }

        /// <summary>
        ///     Returns all uncleared paymentToDelete up to today
        /// </summary>
        /// <returns>list of uncleared transactions</returns>
        public IEnumerable<Payment> GetUnclearedPayments()
        {
            return GetUnclearedPayments(DateTime.Today);
        }

        /// <summary>
        ///     Returns all uncleared paymentToDelete up to the passed date from the database.
        /// </summary>
        /// <returns>list of uncleared transactions</returns>
        public IEnumerable<Payment> GetUnclearedPayments(DateTime date)
        {
            return Data
                .Where(x => !x.IsCleared)
                .Where(x => x.Date.Date <= date.Date)
                .ToList();
        }

        /// <summary>
        ///     returns a list with transactions who are related to this account
        /// </summary>
        /// <param name="account">account to search the related</param>
        /// <returns>List of transactions</returns>
        public IEnumerable<Payment> GetRelatedPayments(Account account)
        {
            return Data.Where(x => x.ChargedAccountId == account.Id
                                   || x.TargetAccountId == account.Id)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        /// <summary>
        ///     returns a list with paymentToDelete who recure in a given timeframe
        /// </summary>
        /// <returns>list of recurring transactions</returns>
        public IEnumerable<Payment> LoadRecurringList(Func<Payment, bool> filter = null)
        {
            var list = Data
                .Where(x => x.IsRecurring && x.ReccuringPaymentId != 0)
                .Where(x => (x.RecurringPayment.IsEndless ||
                             x.RecurringPayment.EndDate >= DateTime.Now.Date)
                            && (filter == null || filter.Invoke(x)))
                .ToList();

            return list
                .Select(x => x.ReccuringPaymentId)
                .Distinct()
                .Select(id => list.Where(x => x.ReccuringPaymentId == id)
                    .OrderByDescending(x => x.Date)
                    .Last())
                .ToList();
        }

        private void DeleteRecurringPaymentIfLastAssociated(Payment item)
        {
            if (Data.All(x => x.ReccuringPaymentId != item.ReccuringPaymentId))
            {
                var recurringList = recurringDataAccess.LoadList(x => x.Id == item.ReccuringPaymentId).ToList();

                foreach (var recTrans in recurringList)
                {
                    recurringDataAccess.DeleteItem(recTrans);
                }
            }
        }
    }
}