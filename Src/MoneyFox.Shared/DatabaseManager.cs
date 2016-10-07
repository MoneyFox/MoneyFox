using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net;
using SQLite.Net.Async;

namespace MoneyFox.Shared
{
    /// <summary>
    ///     Helps with create update and connecting to the database.
    /// </summary>
    public class DatabaseManager : IDatabaseManager
    {
        private const string RECURRING_PAYMENT_CREATE_SCRIPT =
            "CREATE TABLE IF NOT EXISTS RecurringPayments( " +
            "Id INTEGER NOT NULL CONSTRAINT PK_RecurringPayment PRIMARY KEY, " +
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

        private readonly IMvxSqliteConnectionFactory connectionFactory;

        /// <summary>
        ///     Creates a new Database manager object
        /// </summary>
        /// <param name="connectionFactory">The connection factory who creates the connection for each plattform.</param>
        public DatabaseManager(IMvxSqliteConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;

            CreateDatabase();
        }

        /// <summary>
        ///     Creates the config and establish and async connection to access the sqlite database synchronous.
        /// </summary>
        /// <returns>Established SQLiteConnection.</returns>
        public SQLiteConnection GetConnection()
            => connectionFactory.GetConnection(new SqLiteConfig(DatabaseConstants.DB_NAME, false));

        /// <summary>
        ///     Creates a new Database if there isn't already an existing. If there is
        ///     one it tries to update it.
        ///     The update only happens automaticlly on the one who uses the "CreateTable" Command.
        ///     For the others the update has to be done manually.
        /// </summary>
        public void CreateDatabase()
        {
            using (var db = connectionFactory.GetConnection(DatabaseConstants.DB_NAME))
            {
                db.CreateTable<Account>();
                db.CreateTable<Category>();
                db.CreateTable<Payment>();
                db.CreateCommand(RECURRING_PAYMENT_CREATE_SCRIPT).ExecuteNonQuery();
                db.CreateCommand(PAYMENT_TABLE_CREATE_SCRIPT).ExecuteNonQuery();
            }
        }

        /// <summary>
        ///     Creates the config and establish and async connection to access the sqlite database asynchronous.
        /// </summary>
        /// <returns>Established async connection.</returns>
        public SQLiteAsyncConnection GetAsyncConnection()
            => connectionFactory.GetAsyncConnection(new SqLiteConfig(DatabaseConstants.DB_NAME, false));
    }
}