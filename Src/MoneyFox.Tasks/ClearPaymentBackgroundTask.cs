using Windows.ApplicationModel.Background;
using MoneyFox.Shared;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Repositories;
using MoneyManager.Windows.Shortcut;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyFox.Tasks
{
    public sealed class ClearPaymentBackgroundTask : IBackgroundTask
    {
        /// <summary>
        ///     Will Execute the background task, check for payments to clear and update the tile.
        ///     There is no application insight included here since Hockey App has a known issue
        ///     that it won't log errors from background tasks.
        ///     <see cref="http://support.hockeyapp.net/kb/client-integration-windows-and-windows-phone/how-to-instrument-uwp-applications-for-crash-reporting"/>
        /// </summary>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var sqliteConnectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory(),
                new MvxWindowsCommonFileStore());

            var accountRepository = new AccountRepository(new AccountDataAccess(sqliteConnectionCreator));

            new PaymentManager(
                new PaymentRepository(new PaymentDataAccess(sqliteConnectionCreator),
                    new RecurringPaymentDataAccess(sqliteConnectionCreator),
                    accountRepository,
                    new CategoryRepository(new CategoryDataAccess(sqliteConnectionCreator))),
                accountRepository,
                null).ClearPayments();

            Tile.UpdateMainTile();
        }
    }
}