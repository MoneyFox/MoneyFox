using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using MoneyFox.Droid.OneDriveAuth;
using MoneyFox.Shared;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Services;
using MvvmCross.Plugins.File.Droid;
using MvvmCross.Plugins.Sqlite.Droid;

namespace MoneyFox.Droid.Services
{
    [Service]
    public class SyncBackupService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return new Binder();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Task.Run(() => SyncBackups());
            return base.OnStartCommand(intent, flags, startId);
        }

        private async void SyncBackups()
        {
            var filestore = new MvxAndroidFileStore();
            var dbManager = new DatabaseManager(new DroidSqliteConnectionFactory(),
                filestore);

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
                    new OneDriveService(filestore, new OneDriveAuthenticator()), filestore, dbManager),
                new GlobalBusyIndicatorState());

            await autoBackupManager.RestoreBackupIfNewer();
            await autoBackupManager.UploadBackupIfNewer();
        }
    }
}