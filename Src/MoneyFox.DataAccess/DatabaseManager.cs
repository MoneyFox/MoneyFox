using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.Sqlite;
using SQLite;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Helps with create update and connecting to the database.
    /// </summary>
    public class DatabaseManager : IDatabaseManager
    {
        private readonly IMvxSqliteConnectionFactory connectionFactory;
        private readonly IMvxFileStore fileStore;

        /// <summary>
        ///     Creates a new Database manager object
        /// </summary>
        /// <param name="connectionFactory">The connection factory who creates the connection for each plattform.</param>
        /// <param name="fileStore">An FileStore abstraction to access the file system on each plattform.</param>
        public DatabaseManager(IMvxSqliteConnectionFactory connectionFactory, IMvxFileStore fileStore)
        {
            this.connectionFactory = connectionFactory;
            this.fileStore = fileStore;

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
                db.CreateTable<RecurringPayment>();
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