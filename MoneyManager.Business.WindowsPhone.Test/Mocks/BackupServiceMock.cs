using System;
using System.Threading.Tasks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.WindowsPhone.Test.Mocks {
    public class BackupServiceMock : IBackupService {
        public bool IsLoggedIn {
            get { return true; }
        }

        public void Login() {
            throw new NotImplementedException();
        }

        public Task<TaskCompletionType> Upload() {
            throw new NotImplementedException();
        }

        public void Restore() {
            throw new NotImplementedException();
        }

        public Task<DateTime> GetLastCreationDate() {
            return Task.FromResult(new DateTime(2013, 10, 30));
        }
    }
}
