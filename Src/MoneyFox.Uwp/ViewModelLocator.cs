using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using MoneyFox.Presentation.ViewModels.Statistic;
using MoneyFox.Ui.Shared.ViewModels.Backup;
using MoneyFox.Uwp.ViewModels;
using MoneyFox.Uwp.ViewModels.Interfaces;
using MoneyFox.Uwp.ViewModels.Settings;
using MoneyFox.Uwp.ViewModels.Statistic;
using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;

namespace MoneyFox.Uwp
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            if(!ServiceLocator.IsLocationProviderSet && ViewModelBase.IsInDesignModeStatic)
            {
                RegisterServices(new ContainerBuilder());
            }
        }

        public static void RegisterServices(ContainerBuilder registrations)
        {
            IContainer container = registrations.Build();

            if(container != null)
            {
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            }
        }

        //*****************
        //  Data Entry
        //*****************

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
        public static SelectDateRangeDialogViewModel SelectDateRangeDialogVm => ServiceLocator.Current.GetInstance<SelectDateRangeDialogViewModel>();

        //*****************
        //  Statistics
        //*****************
        public static StatisticCashFlowViewModel StatisticCashFlowVm => ServiceLocator.Current.GetInstance<StatisticCashFlowViewModel>();

        public static StatisticCategorySpreadingViewModel StatisticCategorySpreadingVm => ServiceLocator.Current.GetInstance<StatisticCategorySpreadingViewModel>();

        public static StatisticCategorySummaryViewModel StatisticCategorySummaryVm => ServiceLocator.Current.GetInstance<StatisticCategorySummaryViewModel>();

        public static StatisticSelectorViewModel StatisticSelectorVm => ServiceLocator.Current.GetInstance<StatisticSelectorViewModel>();

        //*****************
        //  Settings
        //*****************
        public static WindowsSettingsViewModel SettingsVm => ServiceLocator.Current.GetInstance<WindowsSettingsViewModel>();
    }
}
