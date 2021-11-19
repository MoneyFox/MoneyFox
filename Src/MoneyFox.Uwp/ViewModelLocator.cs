using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using MoneyFox.Uwp.ViewModels;
using MoneyFox.Uwp.ViewModels.About;
using MoneyFox.Uwp.ViewModels.Accounts;
using MoneyFox.Uwp.ViewModels.Categories;
using MoneyFox.Uwp.ViewModels.DataBackup;
using MoneyFox.Uwp.ViewModels.Interfaces;
using MoneyFox.Uwp.ViewModels.Payments;
using MoneyFox.Uwp.ViewModels.Settings;
using MoneyFox.Uwp.ViewModels.Statistics;
using MoneyFox.Uwp.ViewModels.Statistics.StatisticCategorySummary;

namespace MoneyFox.Uwp
{
    public class ViewModelLocator
    {
        protected ViewModelLocator() { }

        //*****************
        //  Data Entry
        //*****************

        public static IAccountListViewModel AccountListVm =>
            ServiceLocator.Current.GetInstance<IAccountListViewModel>();

        public static CategoryListViewModel CategoryListVm =>
            ServiceLocator.Current.GetInstance<CategoryListViewModel>();

        public static PaymentListViewModel PaymentListVm => ServiceLocator.Current.GetInstance<PaymentListViewModel>();

        public static SelectCategoryListViewModel SelectCategoryListVm =>
            ServiceLocator.Current.GetInstance<SelectCategoryListViewModel>();

        public static AddAccountViewModel AddAccountVm => ServiceLocator.Current.GetInstance<AddAccountViewModel>();

        public static AddCategoryViewModel AddCategoryVm => ServiceLocator.Current.GetInstance<AddCategoryViewModel>();

        public static AddPaymentViewModel AddPaymentVm => ServiceLocator.Current.GetInstance<AddPaymentViewModel>();

        public static EditAccountViewModel EditAccountVm => ServiceLocator.Current.GetInstance<EditAccountViewModel>();

        public static EditCategoryViewModel EditCategoryVm =>
            ServiceLocator.Current.GetInstance<EditCategoryViewModel>();

        public static EditPaymentViewModel EditPaymentVm => ServiceLocator.Current.GetInstance<EditPaymentViewModel>();

        public static BackupViewModel BackupVm => ServiceLocator.Current.GetInstance<BackupViewModel>();

        //*****************
        //  Common
        //*****************
        public static SelectDateRangeDialogViewModel SelectDateRangeDialogVm =>
            ServiceLocator.Current.GetInstance<SelectDateRangeDialogViewModel>();

        //*****************
        //  Statistics
        //*****************
        public static StatisticCashFlowViewModel StatisticCashFlowVm =>
            ServiceLocator.Current.GetInstance<StatisticCashFlowViewModel>();

        public static StatisticCategoryProgressionViewModel StatisticCategoryProgressionVm =>
            ServiceLocator.Current.GetInstance<StatisticCategoryProgressionViewModel>();

        public static StatisticAccountMonthlyCashflowViewModel StatisticAccountMonthlyCashflowVm =>
            ServiceLocator.Current.GetInstance<StatisticAccountMonthlyCashflowViewModel>();

        public static StatisticCategorySpreadingViewModel StatisticCategorySpreadingVm =>
            ServiceLocator.Current.GetInstance<StatisticCategorySpreadingViewModel>();

        public static StatisticCategorySummaryViewModel StatisticCategorySummaryVm =>
            ServiceLocator.Current.GetInstance<StatisticCategorySummaryViewModel>();

        public static StatisticSelectorViewModel StatisticSelectorVm =>
            ServiceLocator.Current.GetInstance<StatisticSelectorViewModel>();

        //*****************
        //  Settings
        //*****************
        public static WindowsSettingsViewModel SettingsVm =>
            ServiceLocator.Current.GetInstance<WindowsSettingsViewModel>();

        public static AboutViewModel AboutVm => ServiceLocator.Current.GetInstance<AboutViewModel>();

        public static void RegisterServices(ContainerBuilder registrations)
        {
            IContainer container = registrations.Build();

            if(container != null)
            {
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            }
        }
    }
}