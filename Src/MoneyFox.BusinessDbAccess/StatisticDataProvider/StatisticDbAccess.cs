using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.BusinessDbAccess.QueryObjects;
using MoneyFox.DataAccess.QueryExtensions;
using MoneyFox.DataLayer;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessDbAccess.StatisticDataProvider
{
    public interface IStatisticDbAccess
    {
        /// <summary>
        ///     Selects all payments in the date range without transfers.
        /// </summary>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns>List of Payments.</returns>
        Task<List<Payment>> GetPaymentsWithoutTransfer(DateTime startDate, DateTime endDate);

        /// <summary>
        ///     Selects all Categories including the payments.
        /// </summary>
        /// <returns>List of categories.</returns>
        Task<List<Category>> GetAllCategoriesWithPayments();
    }

    public class StatisticDbAccess : IStatisticDbAccess
    {
        private readonly EfCoreContext context;

        public StatisticDbAccess(EfCoreContext context)
        {
            this.context = context;
        }


        public async Task<List<Payment>> GetPaymentsWithoutTransfer(DateTime startDate, DateTime endDate)
        {
            return await context.Payments
                .Include(x => x.Category)
                .WithoutTransfers()
                .HasDateLargerEqualsThan(startDate.Date)
                .HasDateSmallerEqualsThan(endDate.Date)
                .ToListAsync();
        }

        public async Task<List<Category>> GetAllCategoriesWithPayments()
        {
            return await context.Categories
                .Include(x => x.Payments)
                .OrderByName()
                .ToListAsync();
        }
    }
}
