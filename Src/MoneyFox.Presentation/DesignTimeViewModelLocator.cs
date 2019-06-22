using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.ViewModels.DesignTime;

namespace MoneyFox.Presentation
{
    public static class DesignTimeViewModelLocator
    {
        static ShellViewModel SHELL_VM;
        static DesignTimeAboutViewModel ABOUT_VM;
        static DesignTimeAccountListViewModel ACCOUNT_LIST_VM;
        static DesignTimeStatisticSelectorViewModel STATISTIC_SELECTOR_VIEW_MODEL;
        static DesignTimeStatisticCategorySummaryViewModel STATISTIC_CATEGORY_SUMMARY_VIEW_MODEL;
        static DesignTimeStatisticCategorySpreadingViewModel STATISTIC_CATEGORY_SPREADING_VIEW_MODEL;
        static DesignTimeStatisticCashFlowViewModel STATISTIC_CASH_FLOW_VIEW_MODEL;
        static DesignTimeSettingsPersonalizationViewModel SETTINGS_PERSONALIZATION_VIEW_MODEL;
        static DesignTimeSettingsViewModel SETTINGS_VIEW_MODEL;
        static DesignTimeSettingsBackgroundJobViewModel SETTINGS_BACKGROUND_JOB_VIEW_MODEL;
        static DesignTimeCategoryListViewModel CATEGORY_LIST_VIEW_MODEL;
        static DesignTimePaymentListViewModel PAYMENT_LIST_VIEW_MODEL;
        static DesignTimeBackupViewModel BACKUP_VIEW_MODEL;

        public static ShellViewModel ShellVm => SHELL_VM ?? (SHELL_VM = new ShellViewModel(null));
        public static DesignTimeAboutViewModel AboutVm => ABOUT_VM ?? (ABOUT_VM = new DesignTimeAboutViewModel());
        public static DesignTimeAccountListViewModel AccountListVm => ACCOUNT_LIST_VM ?? (ACCOUNT_LIST_VM = new DesignTimeAccountListViewModel());
        public static DesignTimeStatisticSelectorViewModel StatisticSelectorVm => STATISTIC_SELECTOR_VIEW_MODEL ?? (STATISTIC_SELECTOR_VIEW_MODEL = new DesignTimeStatisticSelectorViewModel());
        public static DesignTimeStatisticCategorySummaryViewModel StatisticCategorySummaryVm => STATISTIC_CATEGORY_SUMMARY_VIEW_MODEL ?? (STATISTIC_CATEGORY_SUMMARY_VIEW_MODEL = new DesignTimeStatisticCategorySummaryViewModel());
        public static DesignTimeStatisticCategorySpreadingViewModel StatisticCategorySpreadingVm => STATISTIC_CATEGORY_SPREADING_VIEW_MODEL ?? (STATISTIC_CATEGORY_SPREADING_VIEW_MODEL = new DesignTimeStatisticCategorySpreadingViewModel());
        public static DesignTimeStatisticCashFlowViewModel StatisticCashFlowVm => STATISTIC_CASH_FLOW_VIEW_MODEL ?? (STATISTIC_CASH_FLOW_VIEW_MODEL = new DesignTimeStatisticCashFlowViewModel());
        public static DesignTimeSettingsPersonalizationViewModel SettingsPersonalizationVm => SETTINGS_PERSONALIZATION_VIEW_MODEL ?? (SETTINGS_PERSONALIZATION_VIEW_MODEL = new DesignTimeSettingsPersonalizationViewModel());
        public static DesignTimeSettingsViewModel SettingsVm => SETTINGS_VIEW_MODEL ?? (SETTINGS_VIEW_MODEL = new DesignTimeSettingsViewModel());
        public static DesignTimeSettingsBackgroundJobViewModel SettingsBackgroundJobVm => SETTINGS_BACKGROUND_JOB_VIEW_MODEL ?? (SETTINGS_BACKGROUND_JOB_VIEW_MODEL = new DesignTimeSettingsBackgroundJobViewModel());
        public static DesignTimeCategoryListViewModel CategoryListVm => CATEGORY_LIST_VIEW_MODEL ?? (CATEGORY_LIST_VIEW_MODEL = new DesignTimeCategoryListViewModel());
        public static DesignTimePaymentListViewModel PaymentListVm => PAYMENT_LIST_VIEW_MODEL ?? (PAYMENT_LIST_VIEW_MODEL = new DesignTimePaymentListViewModel());
        public static DesignTimeBackupViewModel BackupVm => BACKUP_VIEW_MODEL ?? (BACKUP_VIEW_MODEL = new DesignTimeBackupViewModel());
    }
}
