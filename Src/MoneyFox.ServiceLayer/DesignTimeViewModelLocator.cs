using MoneyFox.Business.ViewModels.DesignTime;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Business.ViewModels.Statistic;
using MoneyFox.ServiceLayer.ViewModels.DesignTime;

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
        private static IModifyAccountViewModel MODIFY_ACCOUNT_VIEW_MODEL;
        private static IModifyCategoryViewModel MODIFY_CATEGORY_VIEW_MODEL;
        private static IBalanceViewModel BALANCE_VIEW_MODEL;
        private static IModifyPaymentViewModel MODIFY_PAYMENT_VIEW_MODEL;
        private static ISelectCategoryListViewModel SELECT_CATEGORY_LIST_VIEW_MODEL;
        private static IPaymentListViewModel PAYMENT_LIST_VIEW_MODEL;
        private static IAboutViewModel ABOUT_VIEW_MODEL;
        private static ISelectFilterDialogViewModel SELECT_FILTER_DIALOG_VIEW_MODEL;
        private static ISelectDateRangeDialogViewModel SELECT_DATE_RANGE_DIALOG_VIEW_MODEL;
        private static ISettingsBackgroundJobViewModel SETTINGS_BACKGROUND_JOB_VIEW_MODEL;
        private static ISettingsPersonalizationViewModel SETTINGS_PERSONALIZATION_VIEW_MODEL;

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

        /// <summary>
        ///     Implementation for IModifyAccountViewModel for design time.
        /// </summary>
        public static IModifyAccountViewModel DesignTimeModifyAccountViewModel =>
            MODIFY_ACCOUNT_VIEW_MODEL ?? (MODIFY_ACCOUNT_VIEW_MODEL = new DesignTimeModifyAccountViewModel());

        /// <summary>
        ///     Implementation for IBalanceViewModel for design time.
        /// </summary>
        public static IBalanceViewModel DesignTimeBalanceViewViewModel =>
            BALANCE_VIEW_MODEL ?? (BALANCE_VIEW_MODEL = new DesignTimeBalanceViewViewModel());

        /// <summary>
        ///     Implementation for IModifyPaymentViewModel for design time.
        /// </summary>
        public static IModifyPaymentViewModel DesignTimeModifyPaymentViewModel =>
            MODIFY_PAYMENT_VIEW_MODEL ?? (MODIFY_PAYMENT_VIEW_MODEL = new DesignTimeModifyPaymentViewModel());

        /// <summary>
        ///     Implementation for IModifyCategoryViewModel for design time.
        /// </summary>
        public static IModifyCategoryViewModel DesignTimeModifyCategoryViewModel =>
            MODIFY_CATEGORY_VIEW_MODEL ?? (MODIFY_CATEGORY_VIEW_MODEL = new DesignTimeModifyCategoryViewModel());

        /// <summary>
        ///     Implementation for ISelectCategoryListViewModel for design time.
        /// </summary>
        public static ISelectCategoryListViewModel DesignTimeSelectCategoryListViewModel =>
            SELECT_CATEGORY_LIST_VIEW_MODEL ?? (SELECT_CATEGORY_LIST_VIEW_MODEL = new DesignTimeSelectCategoryListViewModel());

        /// <summary>
        ///     Implementation for IPaymentListViewModel for design time.
        /// </summary>
        public static IPaymentListViewModel DesignTimePaymentListViewModel =>
            PAYMENT_LIST_VIEW_MODEL ?? (PAYMENT_LIST_VIEW_MODEL = new DesignTimePaymentListViewModel());

        /// <summary>
        ///     Implementation for IAboutViewModel for design time.
        /// </summary>
        public static IAboutViewModel DesignTimeAboutViewModel =>
            ABOUT_VIEW_MODEL ?? (ABOUT_VIEW_MODEL = new DesignTimeAboutViewModel());

        /// <summary>
        ///     Implementation for ISelectFilterDialogViewModel for design time.
        /// </summary>
        public static ISelectFilterDialogViewModel DesignTimeSelectFilterDialogViewModel =>
            SELECT_FILTER_DIALOG_VIEW_MODEL ?? (SELECT_FILTER_DIALOG_VIEW_MODEL = new DesignTimeSelectFilterDialogViewModel());

        /// <summary>
        ///     Implementation for ISelectDateRangeDialogViewModel for design time.
        /// </summary>
        public static ISelectDateRangeDialogViewModel DesignTimeSelectDateRangeDialogViewModel =>
            SELECT_DATE_RANGE_DIALOG_VIEW_MODEL ?? (SELECT_DATE_RANGE_DIALOG_VIEW_MODEL = new DesignTimeSelectDateRangeDialogViewModel());

        /// <summary>
        ///     Implementation for ISettingsBackgroundJobViewModel for design time.
        /// </summary>
        public static ISettingsBackgroundJobViewModel DesignTimeSettingsBackgroundJobViewModel =>
            SETTINGS_BACKGROUND_JOB_VIEW_MODEL ?? (SETTINGS_BACKGROUND_JOB_VIEW_MODEL = new DesignTimeSettingsBackgroundJobViewModel());

        /// <summary>
        ///     Implementation for ISettingsPersonalizationViewModel for design time.
        /// </summary>
        public static ISettingsPersonalizationViewModel DesignSettingsPersonalizationViewModel=>
            SETTINGS_PERSONALIZATION_VIEW_MODEL ?? (SETTINGS_PERSONALIZATION_VIEW_MODEL = new DesignTimeSettingsPersonalizationViewModel());
    }
}
