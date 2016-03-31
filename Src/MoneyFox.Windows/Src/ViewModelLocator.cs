using System.Reflection;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Cimbalino.Toolkit.Services;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core;
using MoneyFox.Core.Authentication;
using MoneyFox.Core.Constants;
using MoneyFox.Core.DataAccess;
using MoneyFox.Core.Helpers;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Manager;
using MoneyFox.Core.Repositories;
using MoneyFox.Core.Services;
using MoneyFox.Core.SettingAccess;
using MoneyFox.Core.Shortcut;
using MoneyFox.Core.ViewModels;
using MoneyFox.Windows.Views;
using INavigationService = GalaSoft.MvvmLight.Views.INavigationService;

namespace MoneyFox.Windows
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(CreateNavigationService()).As<INavigationService>();
            builder.RegisterType<EmailComposeService>().As<IEmailComposeService>();
            builder.RegisterType<StoreService>().As<IStoreService>();

            builder.RegisterType<PasswordStorage>().As<IPasswordStorage>();
            builder.RegisterType<Session>().AsSelf();

            builder.RegisterAssemblyTypes(typeof (OneDriveService).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterGeneric(typeof (GenericDataRepository<>))
                .As(typeof (IGenericDataRepository<>))
                .InstancePerDependency();
            builder.RegisterType<Settings>().AsSelf();

            builder.RegisterType<ProtectedData>().As<IProtectedData>();
            builder.RegisterType<AppInformation>().As<IAppInformation>();
            builder.RegisterType<LauncherService>().As<ILauncherService>();
            builder.RegisterType<TileHelper>().AsSelf();
            builder.RegisterType<FileStore>().As<IFileStore>();
            builder.RegisterType<UserNotification>().As<IUserNotification>();
            builder.RegisterType<OneDriveAuthenticator>().As<IOneDriveAuthenticator>();

            builder.RegisterAssemblyTypes(typeof (AccountRepository).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof (PaymentManager).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof (TransferTile).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Tile"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof (AboutViewModel).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsImplementedInterfaces()
                .SingleInstance();

            //TODO: Implement for each ViewModel an Interface
            builder.RegisterAssemblyTypes(typeof (AboutViewModel).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof (Settings).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Settings"))
                .AsImplementedInterfaces()
                .SingleInstance();

            var container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }

        private static PageNavigationService CreateNavigationService()
        {
            var navigationService = new PageNavigationService();
            navigationService.Configure(NavigationConstants.MODIFY_PaymentViewModel_VIEW, typeof (ModifyPaymentView));
            navigationService.Configure(NavigationConstants.MAIN_VIEW, typeof (MainView));
            navigationService.Configure(NavigationConstants.MODIFY_ACCOUNT_VIEW, typeof (ModifyAccountView));
            navigationService.Configure(NavigationConstants.PaymentViewModel_LIST_VIEW, typeof (PaymentViewModelListView));
            navigationService.Configure(NavigationConstants.RECURRING_PaymentViewModel_LIST_VIEW,
                typeof (RecurringPaymentViewModelListView));
            navigationService.Configure(NavigationConstants.SELECT_CATEGORY_LIST_VIEW, typeof (SelectCategoryListView));
            navigationService.Configure(NavigationConstants.STATISTIC_CASH_FLOW_VIEW, typeof (StatisticCashFlowView));
            navigationService.Configure(NavigationConstants.STATISTIC_CATEGORY_SPREADING_VIEW,
                typeof (StatisticCategorySpreadingView));
            navigationService.Configure(NavigationConstants.STATISTIC_CATEGORY_SUMMARY_VIEW,
                typeof (StatisticCategorySummaryView));
            navigationService.Configure(NavigationConstants.STATISTIC_MONTHLY_EXPENSES_VIEW,
                typeof (StatisticMonthlyExpensesView));

            return navigationService;
        }
    }
}