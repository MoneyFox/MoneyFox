using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        Task<IEnumerable<RecurringPayment>> GetPaymentsToRecur();
    }

    /// <inheritdoc />
    public class RecurringPaymentService : IRecurringPaymentService
    {
        private readonly IDbContextScopeFactory dbContextScopeFactory;
        private readonly IRecurringPaymentRepository recurringPaymentRepository;
        private readonly IPaymentRepository paymentRepository;

        /// <summary>
        ///     Constructor
        /// </summary>
        public RecurringPaymentService(IDbContextScopeFactory dbContextScopeFactory, IRecurringPaymentRepository recurringPaymentRepository, IPaymentRepository paymentRepository)
        {
            this.dbContextScopeFactory = dbContextScopeFactory;
            this.recurringPaymentRepository = recurringPaymentRepository;
            this.paymentRepository = paymentRepository;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RecurringPayment>> GetPaymentsToRecur()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var recurringPayments = recurringPaymentRepository
                    .GetMany(x => x.IsEndless ||
                                  x.EndDate >= DateTime.Now.Date)
                    .Include(x => x.RelatedPayments)
                    .Where(x => x.ChargedAccount != null)
                    .ToList();

                var payments = new List<Payment>();
                foreach (var recurringPayment in recurringPayments)
                {
                    // Delete Recurring Payments without assosciated payments. 
                    // This can be removed in later versions, since this will be based on old data.
                    if (!recurringPayment.RelatedPayments.Any())
                    {
                        recurringPaymentRepository.Delete(recurringPayment);
                        continue;
                    }

                    payments.Add(new Payment(
                                     await paymentRepository.GetById(
                                         recurringPayment.RelatedPayments.OrderByDescending(y => y.Date).First().Id)));
                }

                return payments
                    .Where(RecurringPaymentHelper.CheckIfRepeatable)
                    .Select(x => new RecurringPayment(x.Data.RecurringPayment))
                    .ToList();
            }
        }
    }
}
