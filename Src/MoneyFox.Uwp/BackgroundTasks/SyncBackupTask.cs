using System;
using Windows.ApplicationModel.Background;
using MoneyFox.Application.Adapters;
using MoneyFox.Application.CloudBackup;
using MoneyFox.Application.Facades;
using NLog;
using Logger = NLog.Logger;
using CommonServiceLocator;

namespace MoneyFox.Uwp.BackgroundTasks
{
    public class SyncBackupTask
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            logManager.Debug("Sync Backup started.");

            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            if (!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService) return;

            try
            {
                var backupService =ServiceLocator.Current.GetInstance<IBackupService>();
                DateTime backupDate = await backupService.GetBackupDateAsync();

                if (settingsFacade.LastDatabaseUpdate > backupDate) return;

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
