using System;
using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeBackupViewModel : BaseViewModel, IBackupViewModel
    {
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
