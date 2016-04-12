using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net;
using SQLite.Net.Async;

namespace MoneyFox.Shared
{
    /// <summary>
    ///     Provides connections to the database
    /// </summary>
    public class SqliteConnectionCreator : ISqliteConnectionCreator
    {

        private readonly IMvxSqliteConnectionFactory connectionFactory;
        private readonly IMvxFileStore fileStore;

        public SqliteConnectionCreator(IMvxSqliteConnectionFactory connectionFactory, IMvxFileStore fileStore)
        {
            this.connectionFactory = connectionFactory;
            this.fileStore = fileStore;

            CreateDb();
            MigrateDb();
        }

        /// <summary>
        ///     Creates the config and establish and async connection to access the sqlite database synchronous.
        /// </summary>
        /// <returns>Established SQLiteConnection.</returns>
        public SQLiteConnection GetConnection() 
            => connectionFactory.GetConnection(new SqLiteConfig(OneDriveAuthenticationConstants.DB_NAME, false));

        /// <summary>
        ///     Creates the config and establish and async connection to access the sqlite database asynchronous.
        /// </summary>
        /// <returns>Established async connection.</returns>
        public SQLiteAsyncConnection GetAsyncConnection() 
            => connectionFactory.GetAsyncConnection(new SqLiteConfig(OneDriveAuthenticationConstants.DB_NAME, false));

        private void CreateDb()
        {
            using (var db = connectionFactory.GetConnection(OneDriveAuthenticationConstants.DB_NAME))
            {
                db.CreateTable<Account>();
                db.CreateTable<Category>();
                db.CreateCommand(RECURRING_PAYMENT_CREATE_SCRIPT).ExecuteNonQuery();
                db.CreateCommand(PAYMENT_TABLE_CREATE_SCRIPT).ExecuteNonQuery();
            }
        }

        private void MigrateDb()
        {
            if (fileStore.Exists(OneDriveAuthenticationConstants.DB_NAME_OLD))
            {
                using (var dbOld = connectionFactory.GetConnection(OneDriveAuthenticationConstants.DB_NAME_OLD))
                {
                    using (var db = connectionFactory.GetConnection(OneDriveAuthenticationConstants.DB_NAME))
                    {
                        db.InsertAll(dbOld.Table<Account>());
                        db.InsertAll(dbOld.Table<Category>());
                        db.InsertAll(dbOld.Table<RecurringPayment>());
                        db.InsertAll(dbOld.Table<Payment>());
                    }
                }

                fileStore.DeleteFile(OneDriveAuthenticationConstants.DB_NAME_OLD);
            }
        }

        private const string RECURRING_PAYMENT_CREATE_SCRIPT =
            "CREATE TABLE IF NOT EXISTS RecurringPayments( " +
            "Id INTEGER NOT NULL CONSTRAINT PK_RecurringPayment PRIMARY KEY AUTOINCREMENT, " +
            "Amount REAL NOT NULL, " +
            "CategoryId INTEGER, " +
            "ChargedAccountId INTEGER NOT NULL, " +
            "EndDate TEXT NOT NULL, " +
            "IsEndless INTEGER NOT NULL, " +
            "Note TEXT, " +
            "Recurrence INTEGER NOT NULL, " +
            "StartDate TEXT NOT NULL, " +
            "TargetAccountId INTEGER, " +
            "Type INTEGER NOT NULL, " +
            "CONSTRAINT FK_RecurringPayment_Category_CategoryId FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE RESTRICT, " +
            "CONSTRAINT FK_RecurringPayment_Account_ChargedAccountId FOREIGN KEY(ChargedAccountId) REFERENCES Accounts(Id) ON DELETE CASCADE, " +
            "CONSTRAINT FK_RecurringPayment_Account_TargetAccountId FOREIGN KEY(TargetAccountId) REFERENCES Accounts(Id) ON DELETE RESTRICT " +
            ") ";

        private const string PAYMENT_TABLE_CREATE_SCRIPT =
            "CREATE TABLE IF NOT EXISTS Payments( " +
            "Id INTEGER NOT NULL CONSTRAINT PK_Payment PRIMARY KEY AUTOINCREMENT, " +
            "Amount REAL NOT NULL, " +
            "CategoryId INTEGER, " +
            "ChargedAccountId INTEGER NOT NULL, " +
            "Date TEXT NOT NULL, " +
            "IsCleared INTEGER NOT NULL, " +
            "IsRecurring INTEGER NOT NULL, " +
            "Note TEXT, " +
            "RecurringPaymentId INTEGER, " +
            "TargetAccountId INTEGER, " +
            "Type INTEGER NOT NULL, " +
            "CONSTRAINT FK_Payment_Category_CategoryId FOREIGN KEY(CategoryId) REFERENCES Categories(Id) ON DELETE RESTRICT, " +
            "CONSTRAINT FK_Payment_Account_ChargedAccountId FOREIGN KEY(ChargedAccountId) REFERENCES Accounts(Id) ON DELETE CASCADE, " +
            "CONSTRAINT FK_Payment_RecurringPayment_RecurringPaymentId FOREIGN KEY(RecurringPaymentId) REFERENCES RecurringPayments(Id) ON DELETE RESTRICT, " +
            "CONSTRAINT FK_Payment_Account_TargetAccountId FOREIGN KEY(TargetAccountId) REFERENCES Accounts(Id) ON DELETE RESTRICT " +
            ")";
    }
}