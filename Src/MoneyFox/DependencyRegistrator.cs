using System.Linq;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;
using MoneyFox.Business.Authentication;
using MoneyFox.Business.Services;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using MvvmCross.IoC;

namespace MoneyFox
{
    public static class DependencyRegistrator
    {
        public static void RegisterDependencies()
        {
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IDbContextScopeFactory, DbContextScopeFactory>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPasswordStorage, PasswordStorage>();

            typeof(OneDriveService).Assembly.CreatableTypes()
                                  .EndingWith("Service")
                                  .AsInterfaces()
                                  .RegisterAsDynamic();

            typeof(AccountService).Assembly.CreatableTypes()
                                 .EndingWith("Service")
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("Manager")
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            Mvx.IoCProvider.RegisterType(() => new Session(Mvx.IoCProvider.Resolve<ISettingsManager>()));

            typeof(CashFlowDataProvider).Assembly.CreatableTypes()
                                 .EndingWith("Provider")
                                 .AsTypes()
                                 .RegisterAsDynamic();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("Repository")
                                 .AsInterfaces()
                                 .RegisterAsLazySingleton();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("DataProvider")
                                 .AsInterfaces()
                                 .RegisterAsLazySingleton();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("ViewModel")
                                 .AsTypes()
                                 .RegisterAsLazySingleton();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("ViewModel")
                                 .Where(x => !x.Name.StartsWith("DesignTime"))
                                 .AsInterfaces()
                                 .RegisterAsLazySingleton();
        }

    }
}
