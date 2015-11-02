using System.Reflection;
using Beezy.MvvmCross.Plugins.SecureStorage;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Manager;
using MoneyManager.Core.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using Refractored.Xam.Settings.Abstractions;

namespace MoneyManager.Core
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            Mvx.RegisterType<ISqliteConnectionCreator, SqliteConnectionCreator>();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes(typeof (AccountDataAccess).GetTypeInfo().Assembly)
                .EndingWith("DataAccess")
                .AsInterfaces()
                .RegisterAsLazySingleton();

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
                .AsTypes()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("ViewModel")
                .AsTypes()
                .RegisterAsLazySingleton();

            Mvx.RegisterType(() => new PasswordStorage(Mvx.Resolve<IMvxProtectedData>()));
            Mvx.RegisterType(() => new Session(Mvx.Resolve<SettingDataAccess>(), Mvx.Resolve<ISettings>()));

            Mvx.Resolve<RecurringTransactionManager>().CheckRecurringTransactions();
            Mvx.Resolve<TransactionManager>().ClearTransactions();

            // Start the app with the Main View Model.
            RegisterAppStart<MainViewModel>();
        }
    }
}