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
            var dialogService = Mvx.Resolve<IDialogService>();

            dialogService.ShowLoadingDialog();
            await Mvx.Resolve<IBackupManager>().SyncBackup();
            Mvx.Resolve<IRecurringPaymentManager>().CheckRecurringPayments();
            Mvx.Resolve<IPaymentManager>().ClearPayments();
            dialogService.HideLoadingDialog();

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

            Mvx.Resolve<IBackgroundTaskManager>().StartBackgroundTask();
        }
    }
}