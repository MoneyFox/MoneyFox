using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business {
    public class Backup {

        private const string BACKUP_FOLDER_NAME = "MoneyFoxBackup";
        private const string DB_NAME = "moneyfox.sqlite";
        private const string BACKUP_NAME = "backupmoneyfox.sqlite";

        private IBackupService _backupService;

        public Backup(IBackupService backupService){
            _backupService = backupService;
        }
    }
}