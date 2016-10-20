using Windows.ApplicationModel.Background;
using Cheesebaron.MvxPlugins.Settings.WindowsCommon;
using MoneyFox.DataAccess;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Repositories;
using MoneyFox.Windows.Business;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using MoneyFox.Business.Helpers;
using MoneyFox.Business.Manager;
using MoneyFox.Business.Services;

namespace MoneyFox.Windows.Tasks
{
    public sealed class SyncBackupTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                var dbManager = new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWindowsCommonFileStore());

                var accountRepository = new AccountRepository(new AccountDataAccess(dbManager));
                var paymentRepository = new PaymentRepository(new PaymentDataAccess(dbManager));
                var categoryRepository = new CategoryRepository(new CategoryDataAccess(dbManager));

                var settingsManager = new SettingsManager(new WindowsCommonSettings());

                var paymentManager = new PaymentManager(paymentRepository,
                    new AccountRepository(new AccountDataAccess(dbManager)),
                    new RecurringPaymentRepository(new RecurringPaymentDataAccess(dbManager)),
                    null);

                var autoBackupManager = new AutoBackupManager(
                    new BackupManager(
                        new RepositoryManager(paymentManager, accountRepository, paymentRepository, categoryRepository),
                        new OneDriveService(new MvxWindowsCommonFileStore(), new OneDriveAuthenticator()),
                        new MvxWindowsCommonFileStore(), dbManager, settingsManager),
                    new GlobalBusyIndicatorState(), settingsManager);

                await autoBackupManager.RestoreBackupIfNewer();
                await autoBackupManager.UploadBackupIfNewer();
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}