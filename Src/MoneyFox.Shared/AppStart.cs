using MoneyFox.Shared.Authentication;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System.Threading.Tasks;

namespace MoneyFox.Shared
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
        public void Start(object hint = null)
        {
            Task.Run(() => HandleDataOnStartup());
                
            if (Mvx.Resolve<Session>().ValidateSession())
            {
                ShowViewModel<MainViewModel>();
            }
            else
            {
                ShowViewModel<LoginViewModel>();
            }
        }

        private async void HandleDataOnStartup() {
            await Mvx.Resolve<IAutobackupManager>().RestoreBackupIfNewer();

            Mvx.Resolve<IRecurringPaymentManager>().CheckRecurringPayments();
            Mvx.Resolve<IPaymentManager>().ClearPayments();
        }
    }
}