using MoneyFox.Business.Authentication;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

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
            if (Mvx.Resolve<Session>().ValidateSession())
            {
                ShowViewModel<MainViewModel>();
                ShowViewModel<AccountListViewModel>();
                ShowViewModel<MenuViewModel>();
            }
            else
            {
                ShowViewModel<LoginViewModel>();
            }

            await Mvx.Resolve<IAutobackupManager>().RestoreBackupIfNewer();
            Mvx.Resolve<IBackgroundTaskManager>().StartBackgroundTask();
            Mvx.Resolve<IRecurringPaymentManager>().CheckRecurringPayments();
            Mvx.Resolve<IPaymentManager>().ClearPayments();
#pragma warning disable 4014
            Mvx.Resolve<IAutobackupManager>().UploadBackupIfNewer();
#pragma warning restore 4014
        }
    }
}