using System;
using System.Threading.Tasks;

namespace MoneyManager.Foundation.OperationContracts {
    public interface IBackupService {
        void Login();

        Task<TaskCompletionType> Upload();

        void Restore();

        DateTime GetLastCreationDate();
    }
}