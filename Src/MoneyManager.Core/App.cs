using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Manager;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Core
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            Mvx.Resolve<RecurringTransactionManager>().CheckRecurringTransactions();
            Mvx.Resolve<TransactionManager>().ClearTransactions();
            Mvx.Resolve<BalanceViewModel>().UpdateBalance();
            
            // Start the app with the Main View Model.
            RegisterAppStart<MainViewModel>();
        }
    }
}