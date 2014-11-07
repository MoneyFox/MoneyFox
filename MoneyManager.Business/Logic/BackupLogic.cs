using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using BugSense;
using Microsoft.Live;

namespace MoneyManager.Business.Logic{

    public class BackupLogic
    {
        private const string BackUpFolder = "Backups";
        private const string Backupname = "MoneyFoxBackup";
        private const string Databasename = "moneyfox";


        public static async void CreateBackUp(LiveConnectClient liveClient, string backupId)
        {
            try
            {
                if (backupId != null)
                {
                    //var result = MessageBox.Show(AppResources.OverwriteBackupMessage, AppResources.OverwriteBackupTitle, MessageBoxButton.OKCancel);

                    //if (result == MessageBoxResult.Cancel)
                    //{
                    //    return;
                    //}
                }

                //busyProceedAction.IsRunning = true;

                var folderId = await GetFolderId(liveClient, BackUpFolder);
                if (folderId == null)
                {
                    await CreateBackupFolder(liveClient);
                }
                else if (backupId != null)
                {
                    await liveClient.DeleteAsync(backupId);
                }

                //using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                //{
                //    var fileStream = store.OpenFile(Databasename + ".sdf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                //    await liveClient.UploadAsync(folderId, Backupname + ".sdf", fileStream, OverwriteOption.Overwrite);
                //    fileStream.Flush();
                //    fileStream.Close();
                //}
                await CheckForBackup(liveClient, folderId);

                //MessageBox.Show(AppResources.BackupCreatedMessage, AppResources.DoneMessageTitle, MessageBoxButton.OK);
            }
            catch (TaskCanceledException ex)
            {
                BugSenseHandler.Instance.LogException(ex);
                //MessageBox.Show(AppResources.TaskCancelledErrorMessage, AppResources.TaskCancelledErrorTitle, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
                //MessageBox.Show(AppResources.GeneralErrorMessageText, AppResources.GeneralErrorMessageTitle, MessageBoxButton.OK);
            }
        }

        private static async Task CheckForBackup(LiveConnectClient liveClient, string folderId)
        {
            if (folderId != null)
            {
                var backupId = await GetBackupId(liveClient, folderId, Backupname);
                if (backupId != null)
                {
                    //btnRestore.IsEnabled = true;
                    await GetBackupCreationDate(liveClient,backupId);
                }
            }
            else
            {
                //btnRestore.IsEnabled = false;
            }

            //busyProceedAction.IsRunning = false;
        }

        private static async Task<string> CreateBackupFolder(LiveConnectClient liveClient)
        {
            if (liveClient != null)
            {
                try
                {
                    var folderData = new Dictionary<string, object> {{"name", BackUpFolder}};
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
                    return createdAt.ToString("f",
                        new CultureInfo(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
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
            var stream = downloadResult.Stream as MemoryStream;
            //using (IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            //    var myStream = myStore.CreateFile(Databasename + ".sdf");
            //    if (stream != null)
            //    {
            //        myStream.Write(stream.GetBuffer(), 0, (int)stream.Length);
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

                string folderId = await GetFolderId(liveClient, BackUpFolder);
                string backupId = await GetBackupId(liveClient, folderId, Backupname);

                var downloadResult = await DownloadDatabase(liveClient, backupId);
                //DeleteOldData();
                SaveDatabase(downloadResult);
                //ReinitCollections();
                //RecreateReminders();

                //result = MessageBox.Show(AppResources.RestoreCompletedMessage, AppResources.DoneMessageTitle, MessageBoxButton.OKCancel);
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

        private static async Task<string> GetBackupId(LiveConnectClient liveClient, string folderId, string backupName)
        {
            var imageIds = new Dictionary<string, string>();
            try
            {
                //if (folderId == null)
                //{
                //    await GetFolderId(liveClient,BackUpFolder);
                //}

                var operationResultFolder = await liveClient.GetAsync(folderId + "/files");
                dynamic files = operationResultFolder.Result.Values;

                foreach (var data in files)
                {
                    foreach (var file in data)
                    {
                        if (file.name == backupName + ".sqlite")
                        {
                            return file.id;
                        }
                        imageIds.Add(file.id, file.name);
                    }
                }
            }
            catch (LiveConnectException ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
            return String.Empty;
        }

        private static async Task<LiveDownloadOperationResult> DownloadDatabase(LiveConnectClient liveClient,
            string backupId)
        {
            var downloadResult = await liveClient.DownloadAsync(backupId + "/content");

            //busyProceedAction.Content = AppResources.RestoreBackupLabel;
            return downloadResult;
        }
    }
}
