using MoneyFox.Business.ViewModels;
using MoneyFox.Business.ViewModels.DesignTime;

namespace MoneyFox.Business
{
    /// <summary>
    ///     Locator to provide Design Time ViewModels for the Xamarin Forms Designer.
    /// </summary>
    public static class DesignTimeViewModelLocator
    {
        private static IBackupViewModel BACKUP_VIEW_MODEL;

        /// <summary>
        ///     Implementation for IEarlyLabelViewModel for design time.
        /// </summary>
        public static IBackupViewModel DesignTimeBackupViewModel =>
            BACKUP_VIEW_MODEL ?? (BACKUP_VIEW_MODEL = new DesignTimeBackupViewModel());
    }
}
