using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.DataAccess.QueryExtensions;
using MoneyFox.Foundation;

namespace MoneyFox.DataAccess.DataServices
{
    /// <summary>
    ///     Offers service methods to access and modify payment data.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        ///     Returns all uncleared payments up to the passed enddate who are assigned to the passed Account ID.
        /// </summary>
        /// <param name="enddate">Enddate</param>
        /// <param name="accountId">Account to select payments for.</param>
        /// <returns>List of Payments.</returns>
        Task<IEnumerable<Payment>> GetUnclearedPayments(DateTime enddate, int accountId = 0);

        /// <summary>
        ///     Returns all incomes and expenses with the related categories within the passed timeframe.
        /// </summary>
        /// <param name="startdate">Startdate</param>
        /// <param name="enddate">Enddate.</param>
        Task<IEnumerable<Payment>> GetPaymentsWithoutTransfer(DateTime startdate, DateTime enddate);

        /// <summary>
        ///     Returns all payments assosciated to the passed account Id as charged or target.
        /// </summary>
        /// <param name="accountId">Id of the account to load.</param>
        /// <returns>List of Payments</returns>
        Task<IEnumerable<Payment>> GetPaymentsByAccountId(int accountId);

        /// <summary>
        ///     Returns a payment searched by ID.
        /// </summary>
        /// <param name="id">Id to select the payment for.</param>
        /// <returns>The selected payment</returns>
        Task<Payment> GetById(int id);

        /// <summary>
        ///     Saves or updates a one or more payments to the database.
        /// </summary>
        /// <param name="payments">Payments to save or update.</param>
        Task SavePayments(params Payment[] payments);

        /// <summary>
        ///     Deletes a payment from the dataabase.
        /// </summary>
        /// <param name="payment">Payment to delete.</param>
        Task DeletePayment(Payment payment);
    }

    /// <inheritdoc />
    public class PaymentService : IPaymentService
    {
        private readonly IAmbientDbContextLocator ambientDbContextLocator;
        private readonly IDbContextScopeFactory dbContextScopeFactory;

        /// <summary>
        ///     Creates a PaymentService object.
        /// </summary>
        public PaymentService(IAmbientDbContextLocator ambientDbContextLocator, IDbContextScopeFactory dbContextScopeFactory)
        {
            this.dbContextScopeFactory = dbContextScopeFactory;
            this.ambientDbContextLocator = ambientDbContextLocator;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Payment>> GetUnclearedPayments(DateTime enddate, int accountId = 0)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var query = dbContext.Payments
                        .Include(x => x.Category)
                        .Include(x => x.ChargedAccount)
                        .Include(x => x.TargetAccount)
                        .Include(x => x.RecurringPayment)
                        .AreNotCleared()
                        .HasDateSmallerEqualsThan(enddate);

                    if (accountId != 0)
                    {
                        query = query.HasAccountId(accountId);
                    }

                    var list = await query
                        .ToListAsync();

                    return list.Select(x => new Payment(x));
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Payment>> GetPaymentsWithoutTransfer(DateTime startdate, DateTime enddate)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Payments
                                              .Include(x => x.Category)
                                              .WithoutTransfers()
                                              .HasDateLargerEqualsThan(startdate.Date)
                                              .HasDateSmallerEqualsThan(enddate.Date)
                                              .ToListAsync();

                    return list.Select(x => new Payment(x));
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Payment>> GetPaymentsByAccountId(int accountId)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Payments
                        .Include(x => x.Category)
			.Include(x=>x.RecurringPayment)
                        .HasAccountId(accountId)
                        .ToListAsync();
                    return list.Select(x => new Payment(x));
                }
            }
        }

        /// <inheritdoc />
        public async Task<Payment> GetById(int id)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var payment = await dbContext.Payments.Include(x => x.RecurringPayment)
                                                 .Include(x => x.ChargedAccount)
                                                 .Include(x => x.TargetAccount)
                                                 .Include(x => x.Category)
                                                 .FirstAsync(x => x.Id == id);
                    return payment == null ? null : new Payment(payment);
                }
            }
        }

        /// <inheritdoc />
        public async Task SavePayments(params Payment[] payments)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    foreach (var payment in payments)
                    {
                        AddPaymentToChangeSet(dbContext, payment);
                    }
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task DeletePayment(Payment payment)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    PaymentAmountHelper.RemovePaymentAmount(payment);

                    var paymentEntry = dbContext.Entry(payment.Data);
                    paymentEntry.State = EntityState.Deleted;

                    var chargedAccountEntry = dbContext.Entry(payment.Data.ChargedAccount);
                    chargedAccountEntry.State = EntityState.Modified;

                    if (payment.Data.TargetAccount != null)
                    {
                        var targetAccountEntry = dbContext.Entry(payment.Data.TargetAccount);
                        targetAccountEntry.State = EntityState.Modified;
                    }

                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        private void AddPaymentToChangeSet(ApplicationContext dbContext, Payment payment)
        {
            payment.ClearPayment();
            PaymentAmountHelper.AddPaymentAmount(payment);

            if (payment.Data.IsRecurring)
            {
                SaveOrUpdateRecurringPayment(dbContext, payment.Data.RecurringPayment);
            }
            SaveOrUpdatePayment(dbContext, payment.Data);
            SaveOrUpdateAccount(dbContext, payment.Data.ChargedAccount);

            if (payment.Data.Type == PaymentType.Transfer)
            {
                SaveOrUpdateAccount(dbContext, payment.Data.TargetAccount);
            }
        }

        private void SaveOrUpdateRecurringPayment(ApplicationContext dbContext, RecurringPaymentEntity recurringPayment)
        {
            if (recurringPayment.Id != 0)
            {
                recurringPayment.ChargedAccountId = recurringPayment.ChargedAccount.Id;
                recurringPayment.TargetAccountId = recurringPayment.TargetAccount?.Id;
                recurringPayment.CategoryId = recurringPayment.Category?.Id;
            }

            var recurringPaymentEntry = dbContext.Entry(recurringPayment);
            recurringPaymentEntry.State = recurringPayment.Id == 0 ? EntityState.Added : EntityState.Modified;
        }

        private void SaveOrUpdatePayment(ApplicationContext dbContext, PaymentEntity payment)
        {
            if (payment.Id != 0)
            {
                payment.ChargedAccountId = payment.ChargedAccount.Id;
                payment.TargetAccountId = payment.TargetAccount?.Id;
                payment.CategoryId = payment.Category?.Id;
                payment.RecurringPaymentId = payment.RecurringPayment?.Id;
            }

            var paymentEntry = dbContext.Entry(payment);
            paymentEntry.State = payment.Id == 0 ? EntityState.Added : EntityState.Modified;
        }

        private void SaveOrUpdateAccount(ApplicationContext dbContext, AccountEntity account)
        {
            var paymentEntry = dbContext.Entry(account);
            paymentEntry.State = account.Id == 0 ? EntityState.Added : EntityState.Modified;
        }
    }
}
