using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Repositories;
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
        ///     Returns all incomes and expenses within the passed timeframe.
        /// </summary>
        /// <param name="startdate">Startdate</param>
        /// <param name="enddate">Enddate.</param>
        Task<IEnumerable<Payment>> GetPaymentsWithoutTransfer(DateTime startdate, DateTime enddate);

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
        private readonly IPaymentRepository paymentRepository;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        ///     Creates a PaymentService object.
        /// </summary>
        /// <param name="unitOfWork">Instance of <see cref="IUnitOfWork" /></param>
        /// <param name="paymentRepository">Instance of <see cref="IPaymentRepository" /></param>
        public PaymentService(IUnitOfWork unitOfWork, IPaymentRepository paymentRepository)
        {
            this.unitOfWork = unitOfWork;
            this.paymentRepository = paymentRepository;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Payment>> GetUnclearedPayments(DateTime enddate, int accountId = 0)
        {
            var query = paymentRepository
                .GetAll()
                .AreNotCleared()
                .HasDateSmallerEqualsThan(enddate);

            if (accountId != 0)
            {
                query = query.HasAccountId(accountId);
            }

            return await query
                .SelectPayments()
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Payment>> GetPaymentsWithoutTransfer(DateTime startdate, DateTime enddate)
        {
            return await paymentRepository
                .GetAll()
                .WithoutTransfers()
                .HasDateLargerEqualsThan(startdate.Date)
                .HasDateSmallerEqualsThan(enddate.Date)
                .SelectPayments()
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task SavePayment(Payment payment)
        {
            if (payment.Data.Id == 0)
            {
                paymentRepository.Add(payment.Data);
            }
            else
            {
                paymentRepository.Update(payment.Data);
            }
            await unitOfWork.Commit();
        }

        /// <inheritdoc />
        public async Task SavePayments(IEnumerable<Payment> payments)
        {
            foreach (var payment in payments)
            {
                if (payment.Data.Id == 0)
                {
                    paymentRepository.Add(payment.Data);
                }
                else
                {
                    paymentRepository.Update(payment.Data);
                }
            }
            await unitOfWork.Commit();
        }

        /// <inheritdoc />
        public async Task DeletePayment(Payment payment)
        {
            paymentRepository.Delete(payment.Data);
            await unitOfWork.Commit();
        }

        /// <inheritdoc />
        public async Task DeletePayments(IEnumerable<Payment> payments)
        {
            foreach (var payment in payments)
            {
                paymentRepository.Delete(payment.Data);
            }
            await unitOfWork.Commit();
        }
    }
}