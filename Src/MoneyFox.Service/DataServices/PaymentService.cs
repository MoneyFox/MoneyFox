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

        /// <summary>
        ///     Returns all uncleared payments up to the passed enddate.
        /// </summary>
        /// <param name="accountId">Account to select payments for. </param>
        /// <param name="enddate">Enddate</param>
        /// <returns>List of Payments.</returns>
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
    }
}