using MvvmCross.Core.ViewModels;

namespace MoneyFox.Foundation.Interfaces.ViewModels
{
    /// <summary>
    ///     Represents the Actions for a view.
    ///     On Windows this is a normaly in the app bar. 
    ///     On Android for example in a floating action button.
    /// </summary>
    public interface IViewActionViewModel
    {
        MvxCommand GoToAddAccountCommand { get; }

        MvxCommand GoToAddIncomeCommand { get; }

        MvxCommand GoToAddExpenseCommand { get; }

        MvxCommand GoToAddTransferCommand { get; }
    }
}
