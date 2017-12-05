using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Pocos;

namespace MoneyFox.DataAccess.DataServices
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
        private readonly IAmbientDbContextLocator ambientDbContextLocator;
        private readonly IDbContextScopeFactory dbContextScopeFactory;

        /// <summary>
        ///     Constructor
        /// </summary>
        public RecurringPaymentService(IDbContextScopeFactory dbContextScopeFactory, IAmbientDbContextLocator ambientDbContextLocator)
        {
            this.dbContextScopeFactory = dbContextScopeFactory;
            this.ambientDbContextLocator = ambientDbContextLocator;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RecurringPayment>> GetPaymentsToRecur()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var recurringPayments = dbContext.RecurringPayments
                        .Where(x => x.IsEndless ||
                                      x.EndDate >= DateTime.Now.Date)
                        .Include(x => x.RelatedPayments)
                        .Include(x => x.ChargedAccount)
                        .Include(x => x.TargetAccount)
                        .Where(x => x.ChargedAccount != null)
                        .ToList();

                    var payments = new List<Payment>();
                    foreach (var recurringPayment in recurringPayments)
                    {
                        // Delete Recurring Payments without assosciated payments. 
                        // This can be removed in later versions, since this will be based on old data.
                        if (!recurringPayment.RelatedPayments.Any())
                        {
                            var recurringPaymentEntry = dbContext.Entry(recurringPayment);
                            recurringPaymentEntry.State = EntityState.Deleted;
                            continue;
                        }

                        payments.Add(new Payment(
                                         await dbContext.Payments.FindAsync(recurringPayment
                                                                                .RelatedPayments
                                                                                .OrderByDescending(y => y.Date).First()
                                                                                .Id)));
                    }

                    await dbContext.SaveChangesAsync();

                    return payments
                        .Where(RecurringPaymentHelper.CheckIfRepeatable)
                        .Select(x => new RecurringPayment(x.Data.RecurringPayment))
                        .ToList();
                }
            }
        }
    }
}