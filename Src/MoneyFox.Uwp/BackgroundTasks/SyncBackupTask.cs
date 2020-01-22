using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using CommonServiceLocator;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using NLog;

namespace MoneyFox.Uwp.BackgroundTasks
{
    public class SyncBackupTask
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public async Task RunAsync(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            logManager.Debug("Sync Backup started.");

            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            if (!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService) return;

            try
            {
                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();
            }
            catch (Exception ex)
            {
                logManager.Error(ex, "Sync Backup failed.");
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
                logManager.Debug("Sync Backup finished.");
                deferral.Complete();
            }
        }
    }
}
