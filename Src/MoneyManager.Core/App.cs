using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.ViewModels;

namespace MoneyManager.Core
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            Mvx.Resolve<IRecurringTransactionManager>().CheckRecurringTransactions();
            Mvx.Resolve<ITransactionManager>().ClearTransactions();
            Mvx.Resolve<IBalanceViewModel>().UpdateBalance();
            
            // Start the app with the Main View Model.
            RegisterAppStart<MainViewModel>();
        }
    }
}