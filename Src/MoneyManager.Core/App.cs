using System.Linq;
using System.Reflection;
using MoneyManager.Core.Authentication;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace MoneyManager.Core
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override async void Initialize()
        {
            RegisterDependencies();

            // Start the app with the Main View Model.
            RegisterAppStart(new AppStart());

            await Mvx.Resolve<IAutobackupManager>().RestoreBackupIfNewer();
            Mvx.Resolve<IRecurringPaymentManager>().CheckRecurringPayments();
            Mvx.Resolve<IPaymentManager>().ClearPayments();
        }

        private void RegisterDependencies()
        {
            Mvx.RegisterType<ISqliteConnectionCreator, SqliteConnectionCreator>();
            Mvx.RegisterSingleton<IPasswordStorage>(new PasswordStorage(Mvx.Resolve<IProtectedData>()));
            Mvx.RegisterType(() => new Session());

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes(typeof (AccountDataAccess).GetTypeInfo().Assembly)
                .EndingWith("DataAccess")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            // Used for the settings data access who doesn't have an interface
            CreatableTypes(typeof (AccountDataAccess).GetTypeInfo().Assembly)
                .EndingWith("DataAccess")
                .AsTypes()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Repository")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Manager")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("ViewModel")
                .Where(x => !x.Name.StartsWith("DesignTime"))
                .AsInterfaces()
                .RegisterAsLazySingleton();


            CreatableTypes()
                .EndingWith("ViewModel")
                .AsTypes()
                .RegisterAsLazySingleton();
        }
    }
}