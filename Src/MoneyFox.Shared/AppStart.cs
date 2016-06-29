using System.Threading.Tasks;
using MoneyFox.Shared.Authentication;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Shared {
    /// <summary>
    ///     Helper to start the app on all plattforms.
    /// </summary>
    public class AppStart : MvxNavigatingObject, IMvxAppStart {
        /// <summary>
        ///     Execute code on start up.
        /// </summary>
        /// <param name="hint">parameter for the launch of the app.</param>
        public async void Start(object hint = null) {
            if (Mvx.Resolve<Session>().ValidateSession()) {
                ShowViewModel<MainViewModel>();
            }
            else {
                ShowViewModel<LoginViewModel>();
            }
            await Mvx.Resolve<IAutobackupManager>().RestoreBackupIfNewer();
            HandleDataOnStartup();
        }

        private void HandleDataOnStartup() {
            Mvx.Resolve<IRecurringPaymentManager>().CheckRecurringPayments();
            Mvx.Resolve<IPaymentManager>().ClearPayments();
        }
    }
}