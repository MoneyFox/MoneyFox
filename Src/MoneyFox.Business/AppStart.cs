using MoneyFox.Business.Authentication;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;

namespace MoneyFox.Business
{
    /// <summary>
    ///     Helper to start the app on all plattforms.
    /// </summary>
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        /// <summary>
        ///     Execute code on start up.
        /// </summary>
        /// <param name="hint">parameter for the launch of the app.</param>
        public async void Start(object hint = null)
        {
            var filestore = Mvx.Resolve<IMvxFileStore>();
            if (filestore.Exists(DatabaseConstants.DB_NAME_OLD))
            {
                await Mvx.Resolve<IDbFactory>().MigrateOldDatabase(true);
                filestore.DeleteFile(DatabaseConstants.DB_NAME_OLD);
            }

            if (Mvx.Resolve<Session>().ValidateSession())
            {
                ShowViewModel<MenuViewModel>();
				ShowViewModel<AccountListViewModel>();
            }
            else
            {
                ShowViewModel<LoginViewModel>();
            }

            Mvx.Resolve<IBackgroundTaskManager>().StartBackgroundTasks();
        }
    }
}