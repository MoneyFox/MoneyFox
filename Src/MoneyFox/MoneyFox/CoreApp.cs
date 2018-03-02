using EntityFramework.DbContextScope;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Business.Authentication;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

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
                await navigationService.Navigate<MenuViewModel>();
                await navigationService.Navigate<AccountListViewModel>();
            }
            else
            {
                await navigationService.Navigate<LoginViewModel>();
            }
        }
    }
}