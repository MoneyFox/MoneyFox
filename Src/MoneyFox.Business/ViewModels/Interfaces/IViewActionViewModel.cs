using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    /// <summary>
    ///     Represents the Actions for a view.
    ///     On Windows this is a normaly in the app bar. 
    ///     On Android for example in a floating action button.
    /// </summary>
    public interface IViewActionViewModel
    {
        MvxCommand GoToAddIncomeCommand { get; }

        MvxCommand GoToAddExpenseCommand { get; }

        MvxCommand GoToAddTransferCommand { get; }

        /// <summary>
        ///     Indicates if the button to add a new income should be enabled.
        /// </summary>
        bool IsAddIncomeAvailable { get; }

        /// <summary>
        ///     Indicates if the button to add a new expense should be enabled.
        /// </summary>
        bool IsAddExpenseAvailable { get; }

        /// <summary>
        ///     Indicates if the button to add a new transfer should be enabled.
        /// </summary>
        bool IsTransferAvailable { get; }
    }
}
