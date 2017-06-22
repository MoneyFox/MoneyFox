using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.EntityOld;
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
        Task MigrateOldDatabase(bool initMigration = false);
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
        public async Task MigrateOldDatabase(bool initMigration = false)
        {
            if (dbContext == null)
            {
                dbContext = new ApplicationContext();
            }

            if (!initMigration)
            {
                dbContext.Database.ExecuteSqlCommand("DELETE FROM Accounts");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM Categories");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM Payments");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM RecurringPayments");

                dbContext.SaveChanges();
            }
            else
            {
                await dbContext.Database.MigrateAsync();
            }

            using (var dbContextOld = new ApplicationContextOld())
            {
                foreach (var account in dbContextOld.Accounts)
                {
                    dbContext.Accounts.Add(new AccountEntity
                    {
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
                        Name = category.Name,
                        Note = category.Notes,
                    });
                }

                await dbContext.SaveChangesAsync();

                foreach (var recPayment in dbContextOld.RecurringPayments)
                {
                    dbContext.RecurringPayments.Add(new RecurringPaymentEntity
                    {
                        ChargedAccount = dbContext.Accounts.First(x => x.Name == recPayment.ChargedAccount.Name),
                        TargetAccount = recPayment.TargetAccount != null ? dbContext.Accounts.FirstOrDefault(x => x.Name == recPayment.ChargedAccount.Name) : null,
                        Category = recPayment.Category != null ? dbContext.Categories.FirstOrDefault(x => x.Name == recPayment.Category.Name) : null,
                        StartDate = recPayment.StartDate,
                        EndDate = recPayment.EndDate,
                        Amount = recPayment.Amount,
                        Type = (PaymentType)recPayment.Type,
                        Recurrence = (PaymentRecurrence)recPayment.Type,
                        Note = recPayment.Note,
                    });
                }

                await dbContext.SaveChangesAsync();

                foreach (var payment in dbContextOld.Payments)
                {
                    var paymententity = new PaymentEntity
                    {
                        ChargedAccount = dbContext.Accounts.First(x => x.Name == payment.ChargedAccount.Name),
                        TargetAccount = payment.TargetAccount != null
                            ? dbContext.Accounts.FirstOrDefault(x => x.Name == payment.TargetAccount.Name)
                            : null,
                        Category = payment.Category != null
                            ? dbContext.Categories.FirstOrDefault(x => x.Name == payment.Category.Name)
                            : null,
                        Date = payment.Date,
                        Amount = payment.Amount,
                        Type = (PaymentType) payment.Type,
                        Note = payment.Note,
                        IsRecurring = payment.IsRecurring,
                        IsCleared = payment.IsCleared,
                        RecurringPayment = payment.IsRecurring
                            ? dbContext.RecurringPayments
                                        .Include(x => x.ChargedAccount)
                                       .Where(x => Math.Abs(x.Amount - payment.RecurringPayment.Amount) < 0.0001)
                                       .Where(x => x.ChargedAccount.Name == payment.ChargedAccount.Name)
                                       .Where(x => x.StartDate == payment.RecurringPayment.StartDate)
                                       .Where(x => x.EndDate == payment.RecurringPayment.EndDate)
                                       .FirstOrDefault(x => x.Note == payment.RecurringPayment.Note)
                            : null
                    };
                    dbContext.Payments.Add(paymententity);
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