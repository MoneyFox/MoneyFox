using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Service.Pocos;

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

    /// <summary>
    ///     Offers service methods to access and modify payment data.
    /// </summary>
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

        public async Task<IEnumerable<Payment>> GetUnclearedPayments(DateTime enddate, int accountId = 0)
        {
            var query = paymentRepository
                .GetAll()
                .Where(x => !x.IsCleared)
                .Where(p => p.Date.Date <= enddate);

            if (accountId != 0)
            {
                query = query.Where(x => x.ChargedAccountId == accountId || x.TargetAccountId == accountId);
            }

            return await query
                .Select(x => new Payment(x))
                .ToListAsync();
        }

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

        public async Task DeletePayment(Payment payment)
        {
            paymentRepository.Delete(payment.Data);
            await unitOfWork.Commit();
        }

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