using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.Foundation;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;

namespace MoneyFox.DataAccess.Infrastructure
{
    /// <summary>
    ///     Provides an ApplicationContext as Singleton
    /// </summary>
    public interface IDbFactory : IDisposable
    {
        /// <summary>
        ///     Migrates the database and initializes and ApplicationContext if not already one exists and returns it.
        /// </summary>
        /// <returns>Singleton Application Context</returns>
        Task<ApplicationContext> Init();

        /// <summary>
        ///     Migras the data from the old database to the new.
        /// </summary>
        Task MigrateOldDatabase();
    }

    /// <inheritdoc />
    public class DbFactory : Disposable, IDbFactory
    {
        private ApplicationContext dbContext;

        /// <inheritdoc />
        public async Task<ApplicationContext> Init()
        {
            if (dbContext == null)
            {
                dbContext = new ApplicationContext();
            }
            await dbContext.Database.MigrateAsync();
            return dbContext;
        }

        /// <inheritdoc />
        public async Task MigrateOldDatabase()
        {
            if (dbContext == null)
            {
                dbContext = new ApplicationContext();
            }

            using (var dbContextOld = new ApplicationContextOld())
            {
                foreach (var account in dbContextOld.Accounts)
                {
                    dbContext.Accounts.Add(new AccountEntity
                    {
                        Id = account.Id,
                        Name = account.Name,
                        Iban = account.Iban,
                        CurrentBalance = account.CurrentBalance,
                        Note = account.Note,
                        IsExcluded = account.IsExcluded,
                    });
                }

                foreach (var category in dbContextOld.Categories)
                {
                    dbContext.Categories.Add(new CategoryEntity
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Note = category.Notes,
                    });
                }

                foreach (var payment in dbContextOld.Payments)
                {
                    dbContext.Payments.Add(new PaymentEntity()
                    {
                        Id = payment.Id,
                        ChargedAccountId = payment.ChargedAccountId,
                        TargetAccountId = payment.TargetAccountId,
                        CategoryId = payment.CategoryId,
                        Date = payment.Date,
                        Amount = payment.Amount,
                        Type = (PaymentType) payment.Type,
                        Note = payment.Note,
                        IsRecurring = payment.IsRecurring,
                        RecurringPaymentId = payment.RecurringPaymentId
                    });
                }

                foreach (var recPayment in dbContextOld.RecurringPayments)
                {
                    dbContext.RecurringPayments.Add(new RecurringPaymentEntity()
                    {
                        ChargedAccountId = recPayment.ChargedAccountId,
                        TargetAccountId = recPayment.TargetAccountId,
                        CategoryId = recPayment.CategoryId,
                        StartDate = recPayment.StartDate,
                        EndDate = recPayment.EndDate,
                        Amount = recPayment.Amount,
                        Type = (PaymentType)recPayment.Type,
                        Recurrence = (PaymentRecurrence)recPayment.Type,
                        Note = recPayment.Note,
                    });
                }

                await dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        ///     Dispose the current DbFactory
        /// </summary>
        protected override void DisposeCore()
        {
            dbContext?.Dispose();
        }
    }
}