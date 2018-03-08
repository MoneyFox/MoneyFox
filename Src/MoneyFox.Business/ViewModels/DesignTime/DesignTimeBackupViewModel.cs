using System;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeBackupViewModel : BaseViewModel, IBackupViewModel
    {
        public string CreateBackupText => Strings.CreateBackupInformationLabel;
        public string RestoreBackupText => Strings.RestoreBackupInformationLabel;
        public string LoginButtonLabel => Strings.LoginLabel;
        public string LogoutButtonLabel => Strings.LogoutLabel;
        public string CreateBackupButtonLabel => Strings.CreateBackupLabel;
        public string RestoreBackupButtonlabel => Strings.RestoreBackupLabel;
        public string LastBackupTimeStampLabel => Strings.LastBackupDateLabel;
        
        public MvxAsyncCommand LoginCommand { get; }
        public MvxAsyncCommand LogoutCommand { get; }
        public MvxAsyncCommand BackupCommand { get; }
        public MvxAsyncCommand RestoreCommand { get; }
        public DateTime BackupLastModified { get; }

        public bool IsLoadingBackupAvailability { get; }
        public bool IsLoggedIn { get; }
        public bool BackupAvailable { get; }
    }
}
