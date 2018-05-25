using System.Linq;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Business.Authentication;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MoneyFox
{
    /// <summary>
    ///     Entry piont to the Application for MvvmCross.
    /// </summary>
    public class CoreApp : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override async void Initialize()
        {
            RegisterDependencies();

            var navigationService = Mvx.Resolve<IMvxNavigationService>();

            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            using (dbContextScopeFactory.Create())
            {
                await ambientDbContextLocator.Get<ApplicationContext>().Database.MigrateAsync();
            }

            if (Mvx.Resolve<Session>().ValidateSession())
            {
                RegisterAppStart<MainViewModel>();
            } else
            {
                RegisterAppStart<LoginViewModel>();
            }
        }

        private void RegisterDependencies()
        {
            Mvx.ConstructAndRegisterSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
            Mvx.ConstructAndRegisterSingleton<IDbContextScopeFactory, DbContextScopeFactory>();
            Mvx.ConstructAndRegisterSingleton<IPasswordStorage, PasswordStorage>();

            typeof(AccountService).Assembly.CreatableTypes()
                                 .EndingWith("Service")
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("Manager")
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            Mvx.RegisterType(() => new Session(Mvx.Resolve<ISettingsManager>()));

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("Provider")
                                 .AsInterfaces()
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