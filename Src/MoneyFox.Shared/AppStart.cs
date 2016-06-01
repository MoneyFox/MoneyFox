using MoneyFox.Shared.Authentication;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Shared
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            Mvx.Resolve<IAutobackupManager>().RestoreBackupIfNewer();

            Mvx.Resolve<IRecurringPaymentManager>().CheckRecurringPayments();
            Mvx.Resolve<IPaymentManager>().ClearPayments();


            if (Mvx.Resolve<Session>().ValidateSession()) 
            {
                ShowViewModel<MainViewModel>();
            } 
            else 
            {
                ShowViewModel<LoginViewModel>();
            }
        }
    }
}