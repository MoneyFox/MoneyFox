using System;
using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Commands;

namespace MoneyFox.ServiceLayer.ViewModels.DesignTime
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
    }
}
