using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation;
using MoneyFox.Service.Pocos;
using MoneyFox.Service.QueryExtensions;

namespace MoneyFox.Service.DataServices
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
        ///     Saves or updates a payment.
        /// </summary>
        /// <param name="payment">Payment to save or update.</param>
        Task SavePayment(Payment payment);

        /// <summary>
        ///     Saves or updates a list of payments.
        /// </summary>
        /// <param name="payments">Payments to save or update.</param>
        Task SavePayments(IEnumerable<Payment> payments);

        /// <summary>
        ///     Deletes a payment from the dataabase.
        /// </summary>
        /// <param name="payment">Payment to delete.</param>
        Task DeletePayment(Payment payment);

        /// <summary>
        ///     Deletes a list of payments from the dataabase.
        /// </summary>
        /// <param name="payments">Payments to delete.</param>
        Task DeletePayments(IEnumerable<Payment> payments);
    }

    /// <inheritdoc />
    public class PaymentService : IPaymentService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IAmbientDbContextLocator ambientDbContextLocator;
        private readonly IDbContextScopeFactory dbContextScopeFactory;
        private readonly IPaymentRepository paymentRepository;
        private readonly IRecurringPaymentRepository recurringPaymentRepository;

        /// <summary>
        ///     Creates a PaymentService object.
        /// </summary>
        /// <param name="dbContextScopeFactory">Instance of <see cref="IDbContextScopeFactory" /></param>
        /// <param name="paymentRepository">Instance of <see cref="IPaymentRepository" /></param>
        /// <param name="recurringPaymentRepository">Instance of <see cref="IRecurringPaymentRepository" /></param>
        /// <param name="accountRepository">Instance of <see cref="IAccountRepository" /></param>
        public PaymentService(IDbContextScopeFactory dbContextScopeFactory, IPaymentRepository paymentRepository,
                              IRecurringPaymentRepository recurringPaymentRepository,
                              IAccountRepository accountRepository, IAmbientDbContextLocator ambientDbContextLocator)
        {
            this.paymentRepository = paymentRepository;
            this.recurringPaymentRepository = recurringPaymentRepository;
            this.accountRepository = accountRepository;
            this.dbContextScopeFactory = dbContextScopeFactory;
            this.ambientDbContextLocator = ambientDbContextLocator;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Payment>> GetUnclearedPayments(DateTime enddate, int accountId = 0)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var query = paymentRepository
                    .GetAll()
                    .Include(x => x.ChargedAccount)
                    .Include(x => x.TargetAccount)
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

        /// <inheritdoc />
        public async Task<IEnumerable<Payment>> GetPaymentsWithoutTransfer(DateTime startdate, DateTime enddate)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var list = await paymentRepository
                    .GetAll()
                    .Include(x => x.Category)
                    .WithoutTransfers()
                    .HasDateLargerEqualsThan(startdate.Date)
                    .HasDateSmallerEqualsThan(enddate.Date)
                    .ToListAsync();

                return list.Select(x => new Payment(x));
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Payment>> GetPaymentsByAccountId(int accountId)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var list = await paymentRepository
                    .GetAll()
                    .Include(x => x.Category)
                    .HasAccountId(accountId)
                    .ToListAsync();
                return list.Select(x => new Payment(x));
            }
        }

        /// <inheritdoc />
        public async Task<Payment> GetById(int id)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                return new Payment(await paymentRepository.GetById(id));
            }
        }

        /// <inheritdoc />
        public async Task SavePayment(Payment payment)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    AddPaymentToChangeSet(dbContext, payment);
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task SavePayments(IEnumerable<Payment> payments)
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
                PaymentAmountHelper.RemovePaymentAmount(payment);
                paymentRepository.Delete(payment.Data);
                await dbContextScope.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public async Task DeletePayments(IEnumerable<Payment> payments)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                foreach (var payment in payments)
                {
                    paymentRepository.Delete(payment.Data);
                }
                await dbContextScope.SaveChangesAsync();
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
            var recurringPaymentEntry = dbContext.Entry(recurringPayment);
            recurringPaymentEntry.State = recurringPayment.Id == 0 ? EntityState.Added : EntityState.Modified;
        }

        private void SaveOrUpdatePayment(ApplicationContext dbContext, PaymentEntity payment)
        {
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