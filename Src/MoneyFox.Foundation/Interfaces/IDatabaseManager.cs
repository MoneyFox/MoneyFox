using SQLite.Net;

namespace MoneyFox.Foundation.Interfaces
{
    public interface IDatabaseManager
    {
        /// <summary>
        ///     Creates a new Database if there isn't already an existing. If there is
        ///     one it tries to update it.
        ///     The update only happens automatically on the one who uses the "CreateTable" Command.
        ///     For the others the update has to be done manually.
        /// </summary>
        void CreateDatabase();

        /// <summary>
        ///     Establish a Connection to the SQlite database.
        /// </summary>
        /// <returns>Established SQLite Connection.</returns>
        SQLiteConnection GetConnection();

        void MigrateDatabase();
    }
}