using Autofac;
using MediatR;
using MoneyFox.Core;
using MoneyFox.Core.Interfaces;
using MoneyFox.Win.AutoMapper;
using MoneyFox.Win.Infrastructure;
using MoneyFox.Win.Pages;
using MoneyFox.Win.Pages.Accounts;
using MoneyFox.Win.Pages.Categories;
using MoneyFox.Win.Pages.Payments;
using MoneyFox.Win.Pages.Settings;
using MoneyFox.Win.Pages.Statistics;
using MoneyFox.Win.Pages.Statistics.StatisticCategorySummary;
using MoneyFox.Win.Services;
using MoneyFox.Win.ViewModels;
using MoneyFox.Win.ViewModels.Accounts;
using MoneyFox.Win.ViewModels.Categories;
using MoneyFox.Win.ViewModels.DataBackup;
using MoneyFox.Win.ViewModels.Payments;
using MoneyFox.Win.ViewModels.Settings;
using MoneyFox.Win.ViewModels.Statistics;
using MoneyFox.Win.ViewModels.Statistics.StatisticCategorySummary;
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

            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
            builder.RegisterInstance(AutoMapperFactory.Create());

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase))
                .Where(t => !t.Name.Equals("NavigationService", StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces()
                .AsSelf();

            NavigationService.Register<ShellViewModel, ShellPage>();
            NavigationService.Register<AccountListViewModel, AccountListPage>();
            NavigationService.Register<PaymentListViewModel, PaymentListPage>();
            NavigationService.Register<AddPaymentViewModel, AddPaymentPage>();
            NavigationService.Register<EditPaymentViewModel, EditPaymentPage>();
            NavigationService.Register<CategoryListViewModel, CategoryListPage>();
            NavigationService.Register<SettingsViewModel, SettingsHostPage>();
            NavigationService.Register<StatisticCashFlowViewModel, StatisticCashFlowPage>();
            NavigationService.Register<StatisticCategoryProgressionViewModel, StatisticCategoryProgressionPage>();
            NavigationService.Register<StatisticAccountMonthlyCashflowViewModel, StatisticAccountMonthlyCashFlowPage>();
            NavigationService.Register<StatisticCategorySpreadingViewModel, StatisticCategorySpreadingPage>();
            NavigationService.Register<StatisticCategorySummaryViewModel, StatisticCategorySummaryPage>();
            NavigationService.Register<StatisticSelectorViewModel, StatisticSelectorPage>();
            NavigationService.Register<BackupViewModel, BackupPage>();
            NavigationService.Register<WindowsSettingsViewModel, SettingsHostPage>();
        }
    }
}
