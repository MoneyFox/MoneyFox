using System.Linq;
using MoneyFox.Shared.Authentication;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.ResxLocalization;

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
            Mvx.RegisterSingleton(() => new GlobalBusyIndicatorState());
            Mvx.RegisterType<IDatabaseManager, DatabaseManager>();
            Mvx.RegisterSingleton<IPasswordStorage>(new PasswordStorage(Mvx.Resolve<IProtectedData>()));
            Mvx.RegisterSingleton<IMvxTextProvider>(new MvxResxTextProvider(Strings.ResourceManager));

            CreatableTypes()
                .EndingWith("DataAccess")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Repository")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("DataProvider")
                .AsTypes()
                .RegisterAsDynamic();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsDynamic();

            CreatableTypes()
                .EndingWith("Manager")
                .AsInterfaces()
                .RegisterAsDynamic();

            CreatableTypes()
                .EndingWith("ViewModel")
                .Where(x => !x.Name.StartsWith("DesignTime"))
                .AsInterfaces()
                .RegisterAsDynamic();

            CreatableTypes()
                .EndingWith("ViewModel")
                .AsTypes()
                .RegisterAsDynamic();

            Mvx.RegisterType(() => new Session(Mvx.Resolve<ISettingsManager>()));
        }
    }
}