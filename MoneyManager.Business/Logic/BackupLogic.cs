using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using BugSense;
using Microsoft.Live;
using MoneyManager.Foundation;

namespace MoneyManager.Business.Logic
{
    public class BackupLogic
    {
        public static async Task<LiveConnectClient> LogInToOneDrive()
        {
            try
            {
                var authClient = new LiveAuthClient();
                var result = await authClient.LoginAsync(new[] { "wl.signin", "wl.skydrive", "wl.skydrive_update" });

                if (result.Status == LiveConnectSessionStatus.Connected)
                {
                    return new LiveConnectClient(result.Session);
                }
            }
            catch (LiveAuthException ex)
            {
                ShowAuthExceptionMessage();
            }
            catch (LiveConnectException ex)
            {
                ShowConnectExceptionMessage();
            }
            return null;
        }

        private async static Task ShowAuthExceptionMessage()
        {
            var dialog = new MessageDialog(Translation.GetTranslation("AuthExceptionMessage"),
                Translation.GetTranslation("AuthException"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));

            await dialog.ShowAsync();
        }

        private async static Task ShowConnectExceptionMessage()
        {
            var dialog = new MessageDialog(Translation.GetTranslation("ConnectionExceptionMessage"),
                Translation.GetTranslation("ConnectionException"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));

            await dialog.ShowAsync();
        }

        public static async Task<TaskCompletionType> UploadBackup(LiveConnectClient liveClient, string folderId, string dbName)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;

                var storageFile = await localFolder.GetFileAsync(dbName);

                var uploadOperation = await liveClient.CreateBackgroundUploadAsync(
                    folderId, "backup" + dbName, storageFile, OverwriteOption.Overwrite);

                LiveOperationResult uploadResult = await uploadOperation.StartAsync();

                return TaskCompletionType.Successful;
            }
            catch (TaskCanceledException ex)
            {
                BugSenseHandler.Instance.LogException(ex);
                return TaskCompletionType.Aborted;
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
                return TaskCompletionType.Unsuccessful;
            }
        }

        public static async Task<string> CreateBackupFolder(LiveConnectClient liveClient, string folderName)
        {
            if (liveClient != null)
            {
                try
                {
                    var folderData = new Dictionary<string, object> {{"name", folderName}};
                    var operationResult = await liveClient.PostAsync("me/skydrive", folderData);
                    dynamic result = operationResult.Result;
                    return result.id;
                }
                catch (LiveConnectException ex)
                {
                    BugSenseHandler.Instance.LogException(ex);
                }
            }
            return String.Empty;
        }

        public static async Task<string> GetFolderId(LiveConnectClient liveClient, string folderName)
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
                        if (folder.name == folderName)
                        {
                            return folder.id;
                        }
                    }
                }
            }
            catch (LiveConnectException ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
            return String.Empty;
        }

        public static async Task<string> GetBackupId(LiveConnectClient liveClient, string folderId, string backupName)
        {
            try
            {
                var operationResultFolder = await liveClient.GetAsync(folderId + "/files");
                dynamic files = operationResultFolder.Result.Values;

                foreach (var data in files)
                {
                    foreach (var file in data)
                    {
                        if (file.name == backupName)
                        {
                            return file.id;
                        }
                    }
                }
            }
            catch (LiveConnectException ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }

            return String.Empty;
        }

        public static async Task<string> GetBackupCreationDate(LiveConnectClient liveClient, string backupId)
        {
            if (backupId != null)
            {
                try
                {
                    var operationResult =
                        await liveClient.GetAsync(backupId);
                    dynamic result = operationResult.Result;
                    DateTime createdAt = Convert.ToDateTime(result.created_time);
                    return createdAt.ToString("f", new CultureInfo(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
                }
                catch (LiveConnectException ex)
                {
                    BugSenseHandler.Instance.LogException(ex, "Additional Information", "Error getting file info");
                }
                catch (Exception ex)
                {
                    BugSenseHandler.Instance.LogException(ex);
                }
            }
            return String.Empty;
        }

        public static void SaveDatabase(LiveDownloadOperationResult downloadResult)
        {
            //var stream = downloadResult.Stream as MemoryStream;
            //using (IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            //    var myStream = myStore.CreateFile(Databasename + ".sdf");
            //    if (stream != null)
            //    {
            //        myStream.Write(stream.GetBuffer(), 0, (int) stream.Length);
            //        stream.Flush();
            //    }
            //    myStream.Close();
            //}
        }

        public static async void RestoreBackUp(LiveConnectClient liveClient)
        {
            //var result = MessageBox.Show(AppResources.ConfirmRestoreBackupMessage, AppResources.ConfirmRestoreBackupMessageTitle, MessageBoxButton.OKCancel);

            //if (result != MessageBoxResult.OK)
            //{
            //    return;
            //}

            try
            {
                //busyProceedAction.Content = AppResources.LoadBackupLabel;
                //busyProceedAction.IsRunning = true;

                var backupId = String.Empty;

                var downloadResult = await DownloadDatabase(liveClient, backupId);
                //DeleteOldData();
                SaveDatabase(downloadResult);
                //ReinitCollections();
                //RecreateReminders();

                //result = MessageBox.Show(AppResources.RestoreCompletedMessage, AppResources.DoneMessageTitle,
                //    MessageBoxButton.OKCancel);
                //if (result == MessageBoxResult.OK)
                //{
                //    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                //}
            }
            catch (Exception ex)
            {
                //MessageBox.Show(string.Format(AppResources.GeneralErrorMessageText, ex));
                BugSenseHandler.Instance.LogException(ex);
            }
            finally
            {
                //busyProceedAction.IsRunning = false;
            }
        }

        private static async Task<LiveDownloadOperationResult> DownloadDatabase(LiveConnectClient liveClient,
            string backupId)
        {
            var downloadResult = await liveClient.BackgroundDownloadAsync(backupId + "/content");

            //busyProceedAction.Content = AppResources.RestoreBackupLabel;
            return downloadResult;
        }
    }
}
