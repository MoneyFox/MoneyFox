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
        public override async void Initialize()
        {
            // Start the app with the Main View Model.
            RegisterAppStart<MainViewModel>();

            await Mvx.Resolve<IRecurringTransactionManager>().CheckRecurringTransactions();
            Mvx.Resolve<ITransactionManager>().ClearTransactions();
        }
    }
}