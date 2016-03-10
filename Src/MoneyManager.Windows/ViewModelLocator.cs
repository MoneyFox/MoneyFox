using System.Reflection;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Cimbalino.Toolkit.Services;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core;
using MoneyFox.Core.Authentication;
using MoneyFox.Core.Manager;
using MoneyFox.Core.Repositories;
using MoneyFox.Core.Services;
using MoneyFox.Core.ViewModels;
using MoneyFox.DataAccess;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Model;
using MoneyManager.Core.Authentication;
using MoneyManager.DataAccess;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Views;
using INavigationService = GalaSoft.MvvmLight.Views.INavigationService;

namespace MoneyManager.Windows
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

            builder.RegisterAssemblyTypes(typeof(OneDriveService).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            //We have to register them seperatly, otherwise it wasn't able to resolve them.
            //TODO: Find a better way to do this.
            builder.RegisterType<AccountDataAccess>().As<IDataAccess<Account>>();
            builder.RegisterType<PaymentDataAccess>().As<IDataAccess<Payment>>();
            builder.RegisterType<RecurringPaymentDataAccess>().As<IDataAccess<RecurringPayment>>();
            builder.RegisterType<CategoryDataAccess>().As<IDataAccess<Category>>();
            builder.RegisterType<SettingDataAccess>().AsSelf();

            // This is needed for SettingDataAccess
            builder.RegisterAssemblyTypes(typeof(AccountDataAccess).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("DataAccesss"))
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof (AccountRepository).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(PaymentManager).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(AboutViewModel).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsImplementedInterfaces()
                .SingleInstance();

            //TODO: Implement for each ViewModel an Interface
            builder.RegisterAssemblyTypes(typeof(AboutViewModel).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf()
                .SingleInstance();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(builder.Build()));
        }
        private static PageNavigationService CreateNavigationService()
        {
            var navigationService = new PageNavigationService();
            navigationService.Configure(NavigationConstants.MODIFY_PAYMENT_VIEW, typeof(ModifyPaymentView));

            return navigationService;
        }
    }
}