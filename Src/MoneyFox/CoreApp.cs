using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Business.Authentication;
using MoneyFox.Business.Services;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace MoneyFox
{
    /// <summary>
    ///     Entry point to the Application for MvvmCross.
    /// </summary>
    public class CoreApp : MvxApplication
    {
        public static AppPlatform CurrentPlatform { get; set; }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
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
                                 .EndingWith("Adapter")
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
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            using (dbContextScopeFactory.Create())
            {
                ambientDbContextLocator.Get<ApplicationContext>().Database.Migrate();
            }

            if (!Mvx.IoCProvider.CanResolve<Session>()) return;

            if (Mvx.IoCProvider.Resolve<Session>().ValidateSession())
            {
                if (CurrentPlatform == AppPlatform.UWP)
                {
                    RegisterAppStart<AccountListViewModel>();
                } else
                {
                    RegisterAppStart<MainViewModel>();
                }
            } else
            {
                RegisterAppStart<LoginViewModel>();
            }
        }
    }
}