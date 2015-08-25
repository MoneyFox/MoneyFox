using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            Mvx.RegisterType<IDbHelper, DbHelper>();

            CreatableTypes()
                .EndingWith("DataAccess")
                .AsInterfaces()
                .RegisterAsSingleton();

            CreatableTypes()
                .EndingWith("Repository")
                .AsInterfaces()
                .RegisterAsSingleton();

            CreatableTypes()
                .EndingWith("Manager")
                .AsTypes()
                .RegisterAsSingleton();

            CreatableTypes()
                .EndingWith("ViewModel")
                .AsTypes()
                .RegisterAsSingleton();

            // Start the app with the Main View Model.
            RegisterAppStart<MainViewModel>();
        }

    }
}
