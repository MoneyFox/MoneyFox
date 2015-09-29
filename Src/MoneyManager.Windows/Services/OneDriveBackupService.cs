using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Live;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Windows.Services
{
    /// <summary>
    ///     Provides basic operation to create a db backup to OneDrive.
    /// </summary>
    public class OneDriveBackupService : IBackupService
    {
        private string backupId;
        private string folderId;
        private LiveConnectClient liveClient;

        /// <summary>
        ///     Indicates if the user is already logged in or not
        /// </summary>
        public bool IsLoggedIn => liveClient == null;

        /// <summary>
        ///     Prompts a OneDrive login prompt to the user.
        /// </summary>
        public async Task Login()
        {
            var result = await new LiveAuthClient()
                .LoginAsync(new List<string> {"wl.basic", "wl.skydrive", "wl.skydrive_update", "wl.offline_access"});

            if (result.Status == LiveConnectSessionStatus.Connected)
            {
                liveClient = new LiveConnectClient(result.Session);
            }
        }

        /// <summary>
        ///     Uploads a copy of the current database to onedrive
        /// </summary>
        /// <returns>State if the task succeed successfully</returns>
        public async Task<TaskCompletionType> Upload()
        {
            if (liveClient == null)
            {
                await Login();
            }

            if (liveClient == null) return TaskCompletionType.Unsuccessful;

            await GetBackupFolder();

            if (string.IsNullOrEmpty(folderId))
            {
                return TaskCompletionType.Unsuccessful;
            }

            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var storageFile = await localFolder.GetFileAsync(Constants.DB_NAME);

                var uploadOperation = await liveClient.CreateBackgroundUploadAsync(
                    folderId, Constants.BACKUP_NAME, storageFile, OverwriteOption.Overwrite);

                await uploadOperation.StartAsync();

                return TaskCompletionType.Successful;
            }
            catch (TaskCanceledException ex)
            {
                InsightHelper.Report(ex);
                return TaskCompletionType.Aborted;
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
                return TaskCompletionType.Unsuccessful;
            }
        }

        /// <summary>
        ///     Restore a database backup from OneDrive
        /// </summary>
        /// <returns>TaskCompletionType wether the task was successful or not.</returns>
        public async Task<TaskCompletionType> Restore()
        {
            if (liveClient == null)
            {
                await Login();
            }

            if (liveClient == null) return TaskCompletionType.Unsuccessful;

            try
            {
                await GetBackupId();
                var localFolder = ApplicationData.Current.LocalFolder;
                var storageFile =
                    await localFolder.CreateFileAsync(Constants.DB_NAME, CreationCollisionOption.ReplaceExisting);

                await liveClient.BackgroundDownloadAsync(backupId + "/content", storageFile);
                return TaskCompletionType.Successful;
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
                return TaskCompletionType.Unsuccessful;
            }
        }

        private async Task GetBackupId()
        {
            await GetBackupFolder();

            try
            {
                var operationResultFolder = await liveClient.GetAsync(folderId + "/files");
                dynamic files = operationResultFolder.Result.Values;

                foreach (var data in files)
                {
                    foreach (var file in data)
                    {
                        if (file.name == Constants.BACKUP_NAME)
                        {
                            backupId = file.id;
                            break;
                        }
                    }
                }
            }
            catch (LiveConnectException ex)
            {
                InsightHelper.Report(ex);
            }
        }

        private async Task GetBackupFolder()
        {
            try
            {
                var operationResultFolder = await liveClient.GetAsync("me/skydrive/");
                dynamic toplevelfolder = operationResultFolder.Result;

                operationResultFolder = await liveClient.GetAsync(toplevelfolder.id + "/files");
                dynamic folders = operationResultFolder.Result.Values;

                foreach (var data in folders)
                {
                    foreach (var folder in data)
                    {
                        if (folder.name == Constants.BACKUP_FOLDER_NAME)
                        {
                            folderId = folder.id;
                            break;
                        }
                    }
                }
            }
            catch (LiveConnectException ex)
            {
                InsightHelper.Report(ex);
            }
        }
    }
}