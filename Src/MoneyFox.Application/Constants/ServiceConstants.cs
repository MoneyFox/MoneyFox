#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace MoneyFox.Foundation.Constants
{
    /// <summary>
    ///     Contains constants for the services used in the app.
    /// </summary>
    public static class ServiceConstants
    {
        public const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

        /// <summary>
        ///     Maximum number of attempts to sync the database
        /// </summary>
        public const int SYNC_ATTEMPTS = 2;

        /// <summary>
        ///     The amount of time to wait for the OneDrive backup to be completed
        /// </summary>
        public const int BACKUP_OPERATION_TIMEOUT = 10000;

        /// <summary>
        ///     The amount of time to wait before retrying to sync
        /// </summary>
        public const int BACKUP_REPEAT_DELAY = 2000;
    }
}