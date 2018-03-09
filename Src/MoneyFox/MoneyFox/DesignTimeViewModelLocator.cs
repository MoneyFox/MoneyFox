using MoneyFox.Business.ViewModels;
using MoneyFox.Business.ViewModels.DesignTime;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Business.ViewModels.Statistic;

namespace MoneyFox
{
    /// <summary>
    ///     Locator to provide Design Time ViewModels for the Xamarin Forms Designer.
    /// </summary>
    public static class DesignTimeViewModelLocator
    {
        private static IBackupViewModel BACKUP_VIEW_MODEL;
        private static IStatisticSelectorViewModel STATISTIC_SELECTOR_VIEW_MODEL;
        private static IStatisticCategorySummaryViewModel STATISTIC_CATEGORY_SUMMARY_VIEW_MODEL;
        private static ICategoryListViewModel CATEGORY_LIST_VIEW_MODEL;
        private static IAccountListViewModel ACCOUNT_LIST_VIEW_MODEL;
        private static ISettingsViewModel SETTINGS_VIEW_MODEL;

        /// <summary>
        ///     Implementation for IBackupViewModel for design time.
        /// </summary>
        public static IBackupViewModel DesignTimeBackupViewModel =>
            BACKUP_VIEW_MODEL ?? (BACKUP_VIEW_MODEL = new DesignTimeBackupViewModel());

        /// <summary>
        ///     Implementation for IStatisticSelectorViewModel for design time.
        /// </summary>
        public static IStatisticSelectorViewModel DesignTimeStatisticSelectorViewModel =>
            STATISTIC_SELECTOR_VIEW_MODEL ?? (STATISTIC_SELECTOR_VIEW_MODEL = new DesignTimeStatisticSelectorViewModel());

        /// <summary>
        ///     Implementation for IStatisticCategorySummaryViewModel for design time.
        /// </summary>
        public static IStatisticCategorySummaryViewModel DesignTimeStatisticCategorySummaryViewModel =>
            STATISTIC_CATEGORY_SUMMARY_VIEW_MODEL ?? (STATISTIC_CATEGORY_SUMMARY_VIEW_MODEL = new DesignTimeStatisticCategorySummaryViewModel());

        /// <summary>
        ///     Implementation for ICategoryListViewModel for design time.
        /// </summary>
        public static ICategoryListViewModel DesignTimeCategoryListViewModel =>
            CATEGORY_LIST_VIEW_MODEL ?? (CATEGORY_LIST_VIEW_MODEL = new DesignTimeCategoryListViewModel());

        /// <summary>
        ///     Implementation for IAccountListViewModel for design time.
        /// </summary>
        public static IAccountListViewModel DesignTimeAccountListViewModel =>
            ACCOUNT_LIST_VIEW_MODEL ?? (ACCOUNT_LIST_VIEW_MODEL = new DesignTimeAccountListViewModel());

        /// <summary>
        ///     Implementation for ICategoryListViewModel for design time.
        /// </summary>
        public static ISettingsViewModel DesignTimeSettingsViewModel =>
            SETTINGS_VIEW_MODEL ?? (SETTINGS_VIEW_MODEL = new DesignTimeSettingsViewModel());
    }
}
