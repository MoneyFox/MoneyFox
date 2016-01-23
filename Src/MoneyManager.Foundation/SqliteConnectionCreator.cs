using System.Linq;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net;

namespace MoneyManager.Foundation
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
            var db = connectionFactory.GetConnection(new SqLiteConfig(OneDriveAuthenticationConstants.DB_NAME, false));

            var info = db.GetTableInfo("FinancialTransactions");
            if (info.Any())
            {
                CreateDb();
                MigrateTransactions(db);

                db.DropTable<FinancialTransaction>();
                db.DropTable<RecurringTransaction>();
            }

            return db;
        }

        private void MigrateTransactions(SQLiteConnection db)
        {
            var transactions = db.Table<FinancialTransaction>().ToList();
            var recurruringTransactions = db.Table<RecurringTransaction>().ToList();

            foreach (var trans in transactions)
            {
                var payment = new Payment
                {
                    Id = trans.Id,
                    ChargedAccountId = trans.ChargedAccountId,
                    TargetAccountId = trans.TargetAccountId,
                    CategoryId = trans.CategoryId,
                    Date = trans.Date,
                    Amount = trans.Amount,
                    IsCleared = trans.IsCleared,
                    Type = trans.Type,
                    Note = trans.Note,
                    IsRecurring = trans.IsRecurring,
                    RecurringPaymentId = trans.ReccuringTransactionId
                };

                if (trans.IsRecurring)
                {
                    trans.RecurringTransaction = recurruringTransactions.First(x => x.Id == trans.ReccuringTransactionId);

                    payment.RecurringPayment = new RecurringPayment
                    {
                        Id = trans.RecurringTransaction.Id,
                        ChargedAccountId = trans.RecurringTransaction.ChargedAccountId,
                        TargetAccountId = trans.RecurringTransaction.TargetAccountId,
                        CategoryId = trans.RecurringTransaction.CategoryId,
                        StartDate = trans.RecurringTransaction.StartDate,
                        EndDate = trans.RecurringTransaction.EndDate,
                        IsEndless = trans.RecurringTransaction.IsEndless,
                        Amount = trans.RecurringTransaction.Amount,
                        Type = trans.RecurringTransaction.Type,
                        Recurrence = trans.RecurringTransaction.Recurrence,
                        Note = trans.RecurringTransaction.Note
                    };
                }

                db.Insert(payment.RecurringPayment);
                db.Insert(payment);
            }
        }

        private void CreateDb()
        {
            using (var db = connectionFactory.GetConnection("moneyfox.sqlite"))
            {
                db.CreateTable<Account>();
                db.CreateTable<Payment>();
                db.CreateTable<RecurringPayment>();
                db.CreateTable<Category>();
            }
        }
    }
}