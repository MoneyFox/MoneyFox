public class Backup() {

  private const string BackupFolderName = "MoneyFoxBackup";
  private const string DbName = "moneyfox.sqlite";
  private const string BackupName = "backupmoneyfox.sqlite";

  private IBackupService _backupService;


  public Backup(IBackupService backupService){
    _backupService = backupService;
  }
}