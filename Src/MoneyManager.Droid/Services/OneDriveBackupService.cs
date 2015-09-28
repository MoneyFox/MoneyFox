using System;
using System.Threading.Tasks;
using Android.App;
using Microsoft.Live;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Droid.Services
{
    public class OneDriveBackupService : IBackupService
    {
        private const string BACKUP_FOLDER_NAME = "MoneyFoxBackup";
        private const string DB_NAME = "moneyfox.sqlite";
        private const string BACKUP_NAME = "backupmoneyfox.sqlite";
        private string backupId;
        private string folderId;
        private LiveConnectClient liveClient;

        private readonly Activity activity;

        public OneDriveBackupService(Activity activity)
        {
            this.activity = activity;
        }

        public bool IsLoggedIn { get; }

        public async Task Login()
        {
            liveClient = new LiveConnectClient(await new OneDriveClient(activity).LogonAsync());
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

            await GetBackupFolder();

            if (string.IsNullOrEmpty(folderId))
            {
                return TaskCompletionType.Unsuccessful;
            }

            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var storageFile = await localFolder.GetFileAsync(DB_NAME);

                var uploadOperation = await liveClient.CreateBackgroundUploadAsync(
                    folderId, BACKUP_NAME, storageFile, OverwriteOption.Overwrite);

                var uploadResult = await uploadOperation.StartAsync();

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

            try
            {
                await GetBackupId();
                var localFolder = ApplicationData.Current.LocalFolder;
                var storageFile =
                    await localFolder.CreateFileAsync(DB_NAME, CreationCollisionOption.ReplaceExisting);

                await liveClient.BackgroundDownloadAsync(backupId + "/content", storageFile);
                return TaskCompletionType.Successful;
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
                return TaskCompletionType.Unsuccessful;
            }
        }

        /// <summary>
        ///     Returns the Creationtime of an existing backup.
        /// </summary>
        /// <returns>Creationtime as DateTime</returns>
        public async Task<DateTime> GetLastCreationDate()
        {
            if (liveClient == null)
            {
                await Login();
            }

            await GetBackupId();

            if (string.IsNullOrEmpty(backupId))
            {
                return DateTime.MinValue;
            }

            try
            {
                var operationResult =
                    await liveClient.GetAsync(backupId);
                dynamic result = operationResult.Result;
                DateTime createdAt = Convert.ToDateTime(result.created_time);
                return createdAt;
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
                return DateTime.MinValue;
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
                        if (file.name == BACKUP_NAME)
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
                        if (folder.name == BACKUP_FOLDER_NAME)
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