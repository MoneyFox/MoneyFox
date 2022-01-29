using Autofac;
using MoneyFox.Core;
using MoneyFox.Core.Interfaces;
using MoneyFox.Win.Infrastructure;
using MoneyFox.Win.Pages;
using MoneyFox.Win.Pages.Accounts;
using MoneyFox.Win.Pages.Categories;
using MoneyFox.Win.Pages.Payments;
using MoneyFox.Win.Pages.Settings;
using MoneyFox.Win.Pages.Statistics;
using MoneyFox.Win.Services;
using MoneyFox.Win.ViewModels;
using MoneyFox.Win.ViewModels.Accounts;
using MoneyFox.Win.ViewModels.Categories;
using MoneyFox.Win.ViewModels.DataBackup;
using MoneyFox.Win.ViewModels.Payments;
using MoneyFox.Win.ViewModels.Settings;
using MoneyFox.Win.ViewModels.Statistics;
using System;

namespace MoneyFox.Win
{
    internal class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<WinuiInfrastructureModule>();

            builder.RegisterType<NavigationService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GraphClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<ToastService>().AsImplementedInterfaces();
            builder.RegisterType<DialogService>().AsImplementedInterfaces();
            builder.RegisterType<WindowsAppInformation>().AsImplementedInterfaces();
            builder.RegisterType<MarketplaceOperations>().AsImplementedInterfaces();
            builder.RegisterType<WindowsFileStore>().As<IFileStore>();
            builder.RegisterType<ThemeSelectorAdapter>().AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces()
                .AsSelf();

            NavigationService.Register<ShellViewModel, ShellPage>();
            NavigationService.Register<AccountListViewModel, AccountListView>();
            NavigationService.Register<PaymentListViewModel, PaymentListView>();
            NavigationService.Register<AddPaymentViewModel, AddPaymentView>();
            NavigationService.Register<EditPaymentViewModel, EditPaymentView>();
            NavigationService.Register<CategoryListViewModel, CategoryListView>();
            NavigationService.Register<SettingsViewModel, SettingsHostView>();
            //NavigationService.Register<StatisticCashFlowViewModel, StatisticCashFlowView>();
            //NavigationService.Register<StatisticCategoryProgressionViewModel, StatisticCategoryProgressionView>();
            //NavigationService.Register<StatisticAccountMonthlyCashflowViewModel, StatisticAccountMonthlyCashflowView>();
            //NavigationService.Register<StatisticCategorySpreadingViewModel, StatisticCategorySpreadingView>();
            //NavigationService.Register<StatisticCategorySummaryViewModel, StatisticCategorySummaryView>();
            NavigationService.Register<StatisticSelectorViewModel, StatisticSelectorView>();
            NavigationService.Register<BackupViewModel, BackupView>();
            NavigationService.Register<WindowsSettingsViewModel, SettingsHostView>();
        }
    }
}
