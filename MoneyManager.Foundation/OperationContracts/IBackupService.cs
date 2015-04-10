using System;
using System.Threading.Tasks;

namespace MoneyManager.Foundation.OperationContracts {
    public interface IBackupService {
        bool IsLoggedIn { get; }
        
        void Login();

        Task<TaskCompletionType> Upload();

        void Restore();

        Task<DateTime> GetLastCreationDate();
    }
}