using MoneyFox.Shared.Manager;
using Windows.ApplicationModel.Background;
using MoneyFox.Shared;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Services;
using MoneyFox.Windows.Business;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyFox.Windows.Tasks
{
    public sealed class SyncBackupTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var dbManager = new DatabaseManager(new WindowsSqliteConnectionFactory(),
                new MvxWindowsCommonFileStore());

            var accountRepository = new AccountRepository(new AccountDataAccess(dbManager));
            var paymentRepository = new PaymentRepository(new PaymentDataAccess(dbManager));
            var categoryRepository = new CategoryRepository(new CategoryDataAccess(dbManager));

            var paymentManager = new PaymentManager(paymentRepository,
                new AccountRepository(new AccountDataAccess(dbManager)),
                new RecurringPaymentRepository(new RecurringPaymentDataAccess(dbManager)),
                null);
            var autoBackupManager = new AutoBackupManager(
                new BackupManager(
                    new RepositoryManager(paymentManager, accountRepository, paymentRepository, categoryRepository),
                    new OneDriveService(new MvxWindowsCommonFileStore(), new OneDriveAuthenticator()), new MvxWindowsCommonFileStore(), dbManager),
                new GlobalBusyIndicatorState());

            await autoBackupManager.RestoreBackupIfNewer();
            await autoBackupManager.UploadBackupIfNewer();
        }
    }
}