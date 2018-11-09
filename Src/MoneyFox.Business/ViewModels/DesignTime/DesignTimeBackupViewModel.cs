using System;
using System.Globalization;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Resources;
using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeBackupViewModel : IBackupViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public MvxAsyncCommand LoginCommand { get; }
        public MvxAsyncCommand LogoutCommand { get; }
        public MvxAsyncCommand BackupCommand { get; }
        public MvxAsyncCommand RestoreCommand { get; }
        public DateTime BackupLastModified { get; }

        public bool IsLoadingBackupAvailability { get; }
        public bool IsLoggedIn { get; } = true;
        public bool BackupAvailable { get; }
        public DateTime LastExecutionSynBackup { get; }
        public DateTime LastExecutionClearPayments { get; }
        public DateTime LastExecutionCreateRecurringPayments { get; }
    }
}
