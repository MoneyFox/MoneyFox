using System;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Exceptions;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business {
    public class Backup {
        private readonly IBackupService _backupService;

        public Backup(IBackupService backupService){
            _backupService = backupService;
        }

        /// <summary>
        /// Prompts a login screen to the user.
        /// </summary>
        /// <exception cref="ConnectionException">Is thrown if the user couldn't be logged in.</exception>
        public void Login() {
            try {
                _backupService.Login();
            }
            catch (Exception ex) {
                InsightHelper.Report(ex);
                throw new ConnectionException(Translation.GetTranslation("LoginFailedMessage"), ex);
            }
        }
    }
}