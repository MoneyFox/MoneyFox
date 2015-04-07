using System;

namespace MoneyManager.Foundation.OperationContracts {
    public interface IBackupService {
        void Upload();

        void Restore();

        DateTime GetLastCreationDate();
    }
}