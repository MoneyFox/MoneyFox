using Autofac;
using MoneyManager.DataAccess;
using System.Reflection;
using Cirrious.CrossCore;
using MoneyManager.Core.Authentication;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using Module = Autofac.Module;

namespace MoneyManager.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqliteConnectionCreator>().As<ISqliteConnectionCreator>();

            builder.RegisterType<PasswordStorage>().As<IPasswordStorage>();
            builder.RegisterType<Session>().AsSelf();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            //We have to register them seperatly, otherwise it wasn't able to resolve them.
            builder.RegisterType<AccountDataAccess>().As<IDataAccess<Account>>();
            builder.RegisterType<TransactionDataAccess>().As<IDataAccess<FinancialTransaction>>();
            builder.RegisterType<RecurringTransactionDataAccess>().As<IDataAccess<RecurringTransaction>>();
            builder.RegisterType<CategoryDataAccess>().As<IDataAccess<Category>>();
            builder.RegisterType<SettingDataAccess>().AsSelf();

            // This is needed for SettingDataAccess
            builder.RegisterAssemblyTypes(typeof(AccountDataAccess).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("DataAccesss"))
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsImplementedInterfaces()
                .SingleInstance();

            //TODO: Implement for each ViewModel an Interface
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf()
                .SingleInstance();
        }

        public static MainViewModel MainView => Mvx.Resolve<MainViewModel>();
    }
}
