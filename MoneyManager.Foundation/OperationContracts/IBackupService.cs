public interface IBackupService {
  void Upload();

  void Restore();

  DateTime GetLastCreationDate();
}