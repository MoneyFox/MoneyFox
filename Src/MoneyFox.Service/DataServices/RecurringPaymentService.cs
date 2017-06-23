using System;
using System.Collections.Generic;
using System.Linq;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Service.DataServices
{
    /// <summary>
    ///     Offers service methods to access and modify recurring payment data.
    /// </summary>
    public interface IRecurringPaymentService
    {
        /// <summary>
        ///     Selects all recurring payments who are up for a repetition.
        /// </summary>
        /// <returns>List of Payments to recur.</returns>
        IEnumerable<RecurringPayment> GetPaymentsToRecur();
    }

    /// <inheritdoc />
    public class RecurringPaymentService : IRecurringPaymentService
    {
        private readonly IRecurringPaymentRepository recurringPaymentRepository;

        /// <summary>
        ///     Constructor
        /// </summary>
        public RecurringPaymentService(IRecurringPaymentRepository recurringPaymentRepository)
        {
            this.recurringPaymentRepository = recurringPaymentRepository;
        }

        /// <inheritdoc />
        public IEnumerable<RecurringPayment> GetPaymentsToRecur()
        {
            return recurringPaymentRepository
                             .GetMany(x => x.IsEndless ||
                                           x.EndDate >= DateTime.Now.Date)
                             .Where(x => x.ChargedAccount != null)
                             .Select(
                                 x => new Payment(
                                     x.RelatedPayments.OrderByDescending(y => y.Date).First()))
                             .Where(RecurringPaymentHelper.CheckIfRepeatable)
                             .Select(x => new RecurringPayment(x.Data.RecurringPayment))
                             .ToList();
        }
    }
}
