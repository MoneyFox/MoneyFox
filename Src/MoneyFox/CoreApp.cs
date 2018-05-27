using EntityFramework.DbContextScope;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Business.Authentication;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
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

            Mvx.Resolve<IBackgroundTaskManager>().StartBackupSyncTask(60);
        }
    }
}