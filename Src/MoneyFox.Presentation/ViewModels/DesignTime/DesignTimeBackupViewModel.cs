using System;
using System.Globalization;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Commands;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeBackupViewModel : IBackupViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public AsyncCommand InitializeCommand { get; }
        public AsyncCommand LoginCommand { get; }
        public AsyncCommand LogoutCommand { get; }
        public AsyncCommand BackupCommand { get; }
        public AsyncCommand RestoreCommand { get; }
        public DateTime BackupLastModified { get; }

        public bool IsLoadingBackupAvailability { get; }
        public bool IsLoggedIn { get; } = true;
        public bool BackupAvailable { get; }
    }
}
