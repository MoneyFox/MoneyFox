namespace MoneyManager.Foundation.Interfaces
{
    /// <summary>
    ///      Interface for Manager who manages automatic upload and download of backups
    /// </summary>
    public interface IAutobackupManager
    {
        void RestoreBackupIfNewer();
        void UploadBackupIfNewwer();
    }
}