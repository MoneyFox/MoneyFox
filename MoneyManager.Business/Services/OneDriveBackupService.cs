using System; 
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Live;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Services {
    /// <summary>
    /// Provides basic operation to create a db backup to OneDrive.
    /// </summary>
    public class OneDriveBackupService : IBackupService {
        private const string BACKUP_FOLDER_NAME = "MoneyFoxBackup";
        private const string DB_NAME = "moneyfox.sqlite";
        private const string BACKUP_NAME = "backupmoneyfox.sqlite";

        private LiveConnectClient LiveClient { get; set; }
        
        private string _folderId;

        /// <summary>
        /// Prompts a OneDrive login prompt to the user.
        /// </summary>
        public async void Login() {
            LiveLoginResult result = await new LiveAuthClient()
                .LoginAsync(new[] {
                    "wl.basic",
                    "wl.skydrive",
                    "wl.skydrive_update",
                    "wl.offline_access"
                });

            if (result.Status == LiveConnectSessionStatus.Connected) {
                LiveClient = new LiveConnectClient(result.Session);
            }
        }

        public async Task<TaskCompletionType> Upload() {
            await GetBackupFolder();

            if (String.IsNullOrEmpty(_folderId)) {
                return TaskCompletionType.Unsuccessful;
            }

            try {
                var localFolder = ApplicationData.Current.LocalFolder;
                var storageFile = await localFolder.GetFileAsync(DB_NAME);

                var uploadOperation = await LiveClient.CreateBackgroundUploadAsync(
                    _folderId, "backup" + DB_NAME, storageFile, OverwriteOption.Overwrite);

                LiveOperationResult uploadResult = await uploadOperation.StartAsync();

                return TaskCompletionType.Successful;
            } catch (TaskCanceledException ex) {
                InsightHelper.Report(ex);
                return TaskCompletionType.Aborted;
            } catch (Exception ex) {
                InsightHelper.Report(ex);
                return TaskCompletionType.Unsuccessful;
            }
        }

        private async Task GetBackupFolder() {
            try {
                LiveOperationResult operationResultFolder = await LiveClient.GetAsync("me/skydrive/");
                dynamic toplevelfolder = operationResultFolder.Result;

                operationResultFolder = await LiveClient.GetAsync(toplevelfolder.id + "/files");
                dynamic folders = operationResultFolder.Result.Values;

                foreach (dynamic data in folders) {
                    foreach (dynamic folder in data) {
                        if (folder.name == BACKUP_FOLDER_NAME) {
                            _folderId = folder.id;
                        }
                    }
                }
            } catch (LiveConnectException ex) {
                InsightHelper.Report(ex);
            }
        }

        public void Restore() {
            throw new NotImplementedException();
        }

        public DateTime GetLastCreationDate() {
            throw new NotImplementedException();
        }
    }
}
