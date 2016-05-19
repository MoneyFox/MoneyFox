using System.Linq;
using MoneyFox.Shared.Authentication;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Manager;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace MoneyFox.Shared
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            RegisterDependencies();

            // Start the app with the Main View Model.
            RegisterAppStart(new AppStart());
        }

        private void RegisterDependencies()
        {
            Mvx.RegisterType<IDatabaseManager, DatabaseManager>();
            Mvx.RegisterSingleton<IPasswordStorage>(new PasswordStorage(Mvx.Resolve<IProtectedData>()));
            Mvx.RegisterType(() => new Session());

            Mvx.RegisterType<IBackupManager, BackupManager>();
            Mvx.RegisterType<IAutobackupManager, AutoBackupManager>();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("DataAccess")
                .AsInterfaces()
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