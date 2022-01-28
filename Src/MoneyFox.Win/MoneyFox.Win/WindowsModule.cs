using Autofac;
using MoneyFox.Core;
using MoneyFox.Win.Pages;
using MoneyFox.Win.Services;
using MoneyFox.Win.ViewModels;
using System;

namespace MoneyFox.Win
{
    internal class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();

            builder.RegisterType<NavigationService>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces()
                .AsSelf();

            NavigationService.Register<ShellViewModel, ShellPage>();
            //NavigationService.Register<AccountListViewModel, AccountListView>();
            //NavigationService.Register<PaymentListViewModel, PaymentListView>();
            //NavigationService.Register<AddPaymentViewModel, AddPaymentView>();
            //NavigationService.Register<EditPaymentViewModel, EditPaymentView>();
            //NavigationService.Register<CategoryListViewModel, CategoryListView>();
            //NavigationService.Register<SettingsViewModel, SettingsHostView>();
            //NavigationService.Register<StatisticCashFlowViewModel, StatisticCashFlowView>();
            //NavigationService.Register<StatisticCategoryProgressionViewModel, StatisticCategoryProgressionView>();
            //NavigationService.Register<StatisticAccountMonthlyCashflowViewModel, StatisticAccountMonthlyCashflowView>();
            //NavigationService.Register<StatisticCategorySpreadingViewModel, StatisticCategorySpreadingView>();
            //NavigationService.Register<StatisticCategorySummaryViewModel, StatisticCategorySummaryView>();
            //NavigationService.Register<StatisticSelectorViewModel, StatisticSelectorView>();
            //NavigationService.Register<BackupViewModel, BackupView>();
            //NavigationService.Register<WindowsSettingsViewModel, SettingsHostView>();
        }
    }
}
