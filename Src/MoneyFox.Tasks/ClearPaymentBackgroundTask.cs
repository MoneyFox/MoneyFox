using System;
using Windows.ApplicationModel.Background;
using Microsoft.HockeyApp;
using MoneyFox.Shared;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Repositories;
using MoneyManager.Windows.Shortcut;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using MoneyManager.Windows.Services;

namespace MoneyFox.Tasks
{
    public sealed class ClearPaymentBackgroundTask : IBackgroundTask
    {
        private readonly PaymentManager paymentManager;

        public ClearPaymentBackgroundTask()
        {
#if !DEBUG
            HockeyClient.Current.Configure(ServiceConstants.HOCKEY_APP_WINDOWS_ID);
#endif

            HockeyClient.Current.TrackEvent("BackgroundTask");

            var sqliteConnectionCreator = new DatabaseManager(new WindowsSqliteConnectionFactory(), new MvxWindowsCommonFileStore());
            var notificationService = new NotificationService();

            var accountRepository = new AccountRepository(new AccountDataAccess(sqliteConnectionCreator), notificationService);

            paymentManager = new PaymentManager(
                new PaymentRepository(new PaymentDataAccess(sqliteConnectionCreator),
                    new RecurringPaymentDataAccess(sqliteConnectionCreator),
                    accountRepository,
                    new CategoryRepository(new CategoryDataAccess(sqliteConnectionCreator), notificationService),
                    notificationService),
                accountRepository,
                null);
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            paymentManager.ClearPayments();
            Tile.UpdateMainTile();
        }
    }
}