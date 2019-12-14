using System;
using Windows.ApplicationModel.Background;
using Microsoft.Identity.Client;
using MoneyFox.Application.Adapters;
using MoneyFox.Application.CloudBackup;
using MoneyFox.Application.Constants;
using MoneyFox.Application.Facades;
using MoneyFox.Presentation;
using MoneyFox.Uwp.Business;
using NLog;
using Logger = NLog.Logger;

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
                IPublicClientApplication pca = PublicClientApplicationBuilder
                                               .Create(ServiceConstants.MSAL_APPLICATION_ID)
                                               .WithRedirectUri($"msal{ServiceConstants.MSAL_APPLICATION_ID}://auth")
                                               .Build();

                var backupService = new BackupService(new OneDriveService(pca),
                                                      new WindowsFileStore(),
                                                      settingsFacade,
                                                      new ConnectivityAdapter(),
                                                      ViewModelLocator.MessengerInstance);

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
