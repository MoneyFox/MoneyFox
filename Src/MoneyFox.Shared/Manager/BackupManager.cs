using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MvvmCross.Plugins.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Shared.Manager
{
    public interface IBackupManager
    {
        Task Login();

        Task<bool> IsBackupExisting();

        Task<DateTime> GetBackupDate();

        Task UploadNewBackup();

        Task RestoreBackup();
    }

    public class BackupManager : IBackupManager
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly IBackupService backupService;
        private readonly IMvxFileStore fileStore;

        private bool OldBackupRestored = false;

        public BackupManager(IRepositoryManager repositoryManager, IBackupService backupService, IMvxFileStore fileStore)
        {
            this.repositoryManager = repositoryManager;
            this.backupService = backupService;
            this.fileStore = fileStore;
        }

        public async Task<DateTime> GetBackupDate()
        {
            if (await CheckIfUserIsLoggedIn())
            {
                await backupService.GetBackupDate();
            }
            return DateTime.MinValue;
        }
        
        /// <summary>
        ///     Tries to log in the user to the backup service.
        /// </summary>
        ///<exception cref="ConnectionException">Occurs when the backupservice wasn't reachable.</exception>
        ///<exception cref="OneDriveException">Thrown when any error in the OneDrive SDK Occurs</exception>
        public async Task Login()
        {
            await backupService.Login();
        }

        public async Task<bool> IsBackupExisting()
        {
            var files = await backupService.GetFileNames();
            return files.Any();
        }

        public async Task UploadNewBackup()
        {
            if (await CheckIfUserIsLoggedIn())
            {
                await backupService.Upload();
            }
        }

        public async Task RestoreBackup()
        {
            if (await CheckIfUserIsLoggedIn())
            {
                var backupNames = GetBackupName(await backupService.GetFileNames());
                await backupService.Restore(backupNames.Item1, backupNames.Item2);

                if (OldBackupRestored && fileStore.Exists(BackupConstants.DB_NAME))
                {
                    fileStore.DeleteFile(BackupConstants.DB_NAME);
                }

                repositoryManager.ReloadData();
                Settings.LastDatabaseUpdate = DateTime.Now;
            }
        }

        private Tuple<string, string> GetBackupName(List<string> filenames)
        {
            if (filenames.Contains(BackupConstants.BACKUP_NAME))
            {
                return new Tuple<string, string>(BackupConstants.BACKUP_NAME, BackupConstants.DB_NAME);
            }
            OldBackupRestored = true;
            return new Tuple<string, string>(BackupConstants.BACKUP_NAME_OLD, BackupConstants.DB_NAME_OLD);
        }

        private async Task<bool> CheckIfUserIsLoggedIn()
        {
            if (backupService.IsLoggedIn)
            {
                return true;
            }

            await backupService.Login();
            return backupService.IsLoggedIn;
        }
    }
}
