using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataLayer;
using MoneyFox.Foundation;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace MoneyFox.Presentation
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
            //Mvx.IoCProvider.ConstructAndRegisterSingleton<IPasswordStorage, PasswordStorage>();

            //typeof(OneDriveService).Assembly.CreatableTypes()
            //                      .EndingWith("Service")
            //                      .AsInterfaces()
            //                      .RegisterAsDynamic();

            //typeof(AccountService).Assembly.CreatableTypes()
            //                     .EndingWith("Service")
            //                     .AsInterfaces()
            //                     .RegisterAsDynamic();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("Adapter")
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("Manager")
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            //Mvx.IoCProvider.RegisterType(() => new Session(Mvx.IoCProvider.Resolve<ISettingsManager>()));

            //typeof(CashFlowDataProvider).Assembly.CreatableTypes()
            //                     .EndingWith("Provider")
            //                     .AsTypes()
            //                     .RegisterAsDynamic();

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

            new EfCoreContext().Database.Migrate();
            RegisterAppStart<MainViewModel>();

            //if (!Mvx.IoCProvider.CanResolve<Session>()) return;

            //if (Mvx.IoCProvider.Resolve<Session>().ValidateSession())
            //{
            //    if (CurrentPlatform == AppPlatform.UWP)
            //    {
            //        RegisterAppStart<AccountListViewModel>();
            //    } else
            //    {
            //        RegisterAppStart<MainViewModel>();
            //    }
            //} else
            //{
            //    RegisterAppStart<LoginViewModel>();
            //}
        }
    }
}