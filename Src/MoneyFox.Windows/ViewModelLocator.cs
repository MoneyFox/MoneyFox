using System.Reflection;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Cimbalino.Toolkit.Services;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core;
using MoneyFox.Core.Authentication;
using MoneyFox.Core.Helpers;
using MoneyFox.Core.Manager;
using MoneyFox.Core.Repositories;
using MoneyFox.Core.Services;
using MoneyFox.Core.SettingAccess;
using MoneyFox.Core.Shortcut;
using MoneyFox.Core.ViewModels;
using MoneyFox.DataAccess;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Windows;
using INavigationService = GalaSoft.MvvmLight.Views.INavigationService;
using MainView = MoneyFox.Windows.Views.MainView;
using ModifyAccountView = MoneyFox.Windows.Views.ModifyAccountView;
using ModifyPaymentView = MoneyFox.Windows.Views.ModifyPaymentView;
using PaymentListView = MoneyFox.Windows.Views.PaymentListView;
using RecurringPaymentListView = MoneyFox.Windows.Views.RecurringPaymentListView;
using SelectCategoryListView = MoneyFox.Windows.Views.SelectCategoryListView;
using StatisticCashFlowView = MoneyFox.Windows.Views.StatisticCashFlowView;
using StatisticCategorySpreadingView = MoneyFox.Windows.Views.StatisticCategorySpreadingView;
using StatisticCategorySummaryView = MoneyFox.Windows.Views.StatisticCategorySummaryView;
using StatisticMonthlyExpensesView = MoneyFox.Windows.Views.StatisticMonthlyExpensesView;

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
            builder.RegisterType<SqLiteConnectionFactory>().As<ISqliteConnectionFactory>();

            builder.RegisterType<PasswordStorage>().As<IPasswordStorage>();
            builder.RegisterType<Session>().AsSelf();

            builder.RegisterAssemblyTypes(typeof (OneDriveService).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            //We have to register them seperatly, otherwise it wasn't able to resolve them.
            //TODO: Find a better way to do this.
            builder.RegisterType<AccountDataAccess>().As<IDataAccess<Account>>();
            builder.RegisterType<PaymentDataAccess>().As<IDataAccess<Payment>>();
            builder.RegisterType<RecurringPaymentDataAccess>().As<IDataAccess<RecurringPayment>>();
            builder.RegisterType<CategoryDataAccess>().As<IDataAccess<Category>>();
            builder.RegisterType<Settings>().AsSelf();

            builder.RegisterType<ProtectedData>().As<IProtectedData>();
            builder.RegisterType<TileHelper>().AsSelf();
            builder.RegisterType<FileStore>().As<IFileStore>();
            builder.RegisterType<UserNotification>().As<IUserNotification>();

            // This is needed for Settings
            builder.RegisterAssemblyTypes(typeof (AccountDataAccess).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("DataAccesss"))
                .AsSelf()
                .SingleInstance();

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
            navigationService.Configure(NavigationConstants.MODIFY_PAYMENT_VIEW, typeof (ModifyPaymentView));
            navigationService.Configure(NavigationConstants.MAIN_VIEW, typeof (MainView));
            navigationService.Configure(NavigationConstants.MODIFY_ACCOUNT_VIEW, typeof (ModifyAccountView));
            navigationService.Configure(NavigationConstants.PAYMENT_LIST_VIEW, typeof (PaymentListView));
            navigationService.Configure(NavigationConstants.RECURRING_PAYMENT_LIST_VIEW,
                typeof (RecurringPaymentListView));
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