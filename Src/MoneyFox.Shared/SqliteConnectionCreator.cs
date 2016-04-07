using System.IO;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net;

namespace MoneyFox.Shared
{
    public class SqliteConnectionCreator : ISqliteConnectionCreator
    {
        private readonly IMvxSqliteConnectionFactory connectionFactory;

        public SqliteConnectionCreator(IMvxSqliteConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;

            CreateDb();
        }

        /// <summary>
        ///     Creates the config and establishe the connection to the sqlite database.
        /// </summary>
        /// <returns>Established SQLiteConnection.</returns>
        public SQLiteConnection GetConnection()
        {
            return connectionFactory.GetConnection(new SqLiteConfig(OneDriveAuthenticationConstants.DB_NAME, false));
        }

        private void CreateDb()
        {
            using (var db = connectionFactory.GetConnection(OneDriveAuthenticationConstants.DB_NAME))
            {
                db.CreateTable<Account>();
                db.CreateTable<Payment>();
                db.CreateTable<RecurringPayment>();
                db.CreateTable<Category>();
            }
        }

        //private async void MigrateDatabase()
        //{
        //    using (var db = new MoneyFoxDataContext())
        //    {
        //        db.Database.Migrate();

        //        var filestore = new FileStore();

        //        if (!await filestore.Exists(Path.Combine(ApplicationData.Current.LocalFolder.Path,
        //            OneDriveConstants.DB_NAME_OLD)))
        //        {
        //            return;
        //        }

        //        using (var oldDb = new MoneyFoxOldDataContext())
        //        {
        //            foreach (var oldCategory in oldDb.Categories)
        //            {
        //                db.Categories.Add(new Category
        //                {
        //                    Id = oldCategory.Id,
        //                    Name = oldCategory.Name
        //                });
        //            }

        //            foreach (var oldAccount in oldDb.Accounts)
        //            {
        //                db.Accounts.Add(new Account
        //                {
        //                    Id = oldAccount.Id,
        //                    Name = oldAccount.Name,
        //                    CurrentBalance = oldAccount.CurrentBalance,
        //                    Iban = oldAccount.Iban,
        //                    Note = oldAccount.Note
        //                });
        //            }

        //            foreach (var oldPayment in oldDb.Payments)
        //            {
        //                db.Payments.Add(new Payment
        //                {
        //                    Id = oldPayment.Id,
        //                    ChargedAccountId = oldPayment.ChargedAccountId,
        //                    TargetAccountId = oldPayment.TargetAccountId,
        //                    Amount = oldPayment.Amount,
        //                    CategoryId = oldPayment.CategoryId,
        //                    Date = oldPayment.Date,
        //                    IsCleared = oldPayment.IsCleared,
        //                    IsRecurring = oldPayment.IsRecurring,
        //                    Note = oldPayment.Note,
        //                    RecurringPaymentId = oldPayment.RecurringPaymentId,
        //                    Type = oldPayment.Type
        //                });
        //            }

        //            foreach (var oldRecPayments in oldDb.RecurringPayments)
        //            {
        //                db.RecurringPayments.Add(new RecurringPayment
        //                {
        //                    Id = oldRecPayments.Id,
        //                    Amount = oldRecPayments.Amount,
        //                    CategoryId = oldRecPayments.CategoryId,
        //                    ChargedAccountId = oldRecPayments.ChargedAccountId,
        //                    TargetAccountId = oldRecPayments.TargetAccountId,
        //                    Recurrence = oldRecPayments.Recurrence,
        //                    IsEndless = oldRecPayments.IsEndless,
        //                    Note = oldRecPayments.Note,
        //                    StartDate = oldRecPayments.StartDate,
        //                    EndDate = oldRecPayments.EndDate,
        //                    Type = oldRecPayments.Type
        //                });
        //            }

        //            oldDb.Database.EnsureDeleted();
        //        }
        //    }
        //}

    }
}