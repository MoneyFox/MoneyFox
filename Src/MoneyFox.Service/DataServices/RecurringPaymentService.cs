using System;
using System.Collections.Generic;
using System.Linq;
using MoneyFox.DataAccess;
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
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        ///     Constructor
        /// </summary>
        public RecurringPaymentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public IEnumerable<RecurringPayment> GetPaymentsToRecur()
        {
            return unitOfWork.RecurringPaymentRepository
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
