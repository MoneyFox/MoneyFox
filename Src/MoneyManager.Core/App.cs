using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Core
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            // Start the app with the Main View Model.
            RegisterAppStart<MainViewModel>();

            Mvx.Resolve<IRecurringTransactionManager>().CheckRecurringPayments();
            Mvx.Resolve<IPaymentManager>().ClearPayments();
        }
    }
}