namespace MoneyFox.Win;

using System;
using Autofac;
using Core;
using Core.Interfaces;
using Infrastructure;
using Mapping;
using MediatR;
using Pages;
using Pages.Accounts;
using Pages.Categories;
using Pages.Payments;
using Pages.Settings;
using Pages.Statistics;
using Pages.Statistics.StatisticCategorySummary;
using Services;
using ViewModels;
using ViewModels.Accounts;
using ViewModels.Categories;
using ViewModels.DataBackup;
using ViewModels.Payments;
using ViewModels.Settings;
using ViewModels.Statistics;
using ViewModels.Statistics.StatisticCategorySummary;

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
            .Where(t => t.Name.EndsWith(value: "Service", comparisonType: StringComparison.CurrentCultureIgnoreCase))
            .Where(t => !t.Name.Equals(value: "NavigationService", comparisonType: StringComparison.CurrentCultureIgnoreCase))
            .AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(t => !t.Name.StartsWith(value: "DesignTime", comparisonType: StringComparison.CurrentCultureIgnoreCase))
            .Where(t => t.Name.EndsWith(value: "ViewModel", comparisonType: StringComparison.CurrentCultureIgnoreCase))
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
