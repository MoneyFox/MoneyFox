using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.ViewModels.Interfaces;
using MoneyFox.Presentation.ViewModels.Settings;
using MoneyFox.Presentation.ViewModels.Statistic;

namespace MoneyFox.Presentation
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            if(!ServiceLocator.IsLocationProviderSet && ViewModelBase.IsInDesignModeStatic)
                RegisterServices(new ContainerBuilder());
        }

        public static void RegisterServices(ContainerBuilder registrations)
        {
            IContainer container = registrations.Build();

            if(container != null)
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }

        public static string MainPage => nameof(ShellViewModel);

        public static string AccountList => nameof(AccountListViewModel);

        public static string PaymentList => nameof(PaymentListViewModel);

        public static string CategoryList => nameof(CategoryListViewModel);

        public static string SelectCategoryList => nameof(SelectCategoryListViewModel);

        public static string AddAccount => nameof(AddAccountViewModel);

        public static string AddCategory => nameof(AddCategoryViewModel);

        public static string AddPayment => nameof(AddPaymentViewModel);

        public static string EditAccount => nameof(EditAccountViewModel);

        public static string EditCategory => nameof(EditCategoryViewModel);

        public static string EditPayment => nameof(EditPaymentViewModel);

        public static string Backup => nameof(BackupViewModel);

        //*****************
        //  Statistics
        //*****************
        public static string StatisticSelector => nameof(StatisticSelectorViewModel);

        public static string StatisticCashFlow => nameof(StatisticCashFlowViewModel);

        public static string StatisticCategorySpreading => nameof(StatisticCategorySpreadingViewModel);

        public static string StatisticCategorySummary => nameof(StatisticCategorySummaryViewModel);
        public static string PaymentForCategoryList => nameof(PaymentForCategoryListViewModel);


        //*****************
        //  Settings
        //*****************
        public static string About => nameof(AboutViewModel);

        public static string SettingsRegional => nameof(RegionalSettingsViewModel);

        public static string Settings => nameof(SettingsViewModel);

        public static string SettingsBackgroundJob => nameof(SettingsBackgroundJobViewModel);

        public static string SettingsPersonalization => nameof(SettingsPersonalizationViewModel);

        public static ShellViewModel ShellVm => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public static IAccountListViewModel AccountListVm => ServiceLocator.Current.GetInstance<IAccountListViewModel>();

        public static CategoryListViewModel CategoryListVm => ServiceLocator.Current.GetInstance<CategoryListViewModel>();

        public static PaymentListViewModel PaymentListVm => ServiceLocator.Current.GetInstance<PaymentListViewModel>();

        public static SelectCategoryListViewModel SelectCategoryListVm => ServiceLocator.Current.GetInstance<SelectCategoryListViewModel>();

        public static AddAccountViewModel AddAccountVm => ServiceLocator.Current.GetInstance<AddAccountViewModel>();

        public static AddCategoryViewModel AddCategoryVm => ServiceLocator.Current.GetInstance<AddCategoryViewModel>();

        public static AddPaymentViewModel AddPaymentVm => ServiceLocator.Current.GetInstance<AddPaymentViewModel>();

        public static EditAccountViewModel EditAccountVm => ServiceLocator.Current.GetInstance<EditAccountViewModel>();

        public static EditCategoryViewModel EditCategoryVm => ServiceLocator.Current.GetInstance<EditCategoryViewModel>();

        public static EditPaymentViewModel EditPaymentVm => ServiceLocator.Current.GetInstance<EditPaymentViewModel>();

        public static BackupViewModel BackupVm => ServiceLocator.Current.GetInstance<BackupViewModel>();

        //*****************
        //  Common
        //*****************
        public static SelectDateRangeDialogViewModel SelectDateRangeDialogVm
                                                     => ServiceLocator.Current.GetInstance<SelectDateRangeDialogViewModel>();

        public static SelectFilterDialogViewModel SelectFilterDialogVm => ServiceLocator.Current.GetInstance<SelectFilterDialogViewModel>();

        //*****************
        //  Statistics
        //*****************
        public static StatisticSelectorViewModel StatisticSelectorVm => ServiceLocator.Current.GetInstance<StatisticSelectorViewModel>();

        public static StatisticCashFlowViewModel StatisticCashFlowVm => ServiceLocator.Current.GetInstance<StatisticCashFlowViewModel>();

        public static StatisticCategorySpreadingViewModel StatisticCategorySpreadingVm
                                                          => ServiceLocator.Current.GetInstance<StatisticCategorySpreadingViewModel>();

        public static StatisticCategorySummaryViewModel StatisticCategorySummaryVm
                                                        => ServiceLocator.Current.GetInstance<StatisticCategorySummaryViewModel>();

        //*****************
        //  Settings
        //*****************
        public static SettingsViewModel SettingsVm => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public static SettingsBackgroundJobViewModel SettingsBackgroundVm
                                                     => ServiceLocator.Current.GetInstance<SettingsBackgroundJobViewModel>();

        public static SettingsPersonalizationViewModel SettingsPersonalizationVm
                                                       => ServiceLocator.Current.GetInstance<SettingsPersonalizationViewModel>();

        public static AboutViewModel AboutVm => ServiceLocator.Current.GetInstance<AboutViewModel>();

        public static RegionalSettingsViewModel RegionalSettingsVm => ServiceLocator.Current.GetInstance<RegionalSettingsViewModel>();
    }
}
