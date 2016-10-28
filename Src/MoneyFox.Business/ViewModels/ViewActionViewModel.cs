using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Represents the Actions for a view.
    ///     On Windows this is a normaly in the app bar. 
    ///     On Android for example in a floating action button.
    /// </summary>
    public class ViewActionViewModel : BaseViewModel, IViewActionViewModel
    {
        public MvxCommand GoToAddAccountCommand => 
                new MvxCommand(() => ShowViewModel<ModifyAccountViewModel>(new { isEdit = false, selectedAccountId = 0 }));

        public MvxCommand GoToAddIncomeCommand =>
                new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Income}));
        public MvxCommand GoToAddExpenseCommand =>
                new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Expense }));
        public MvxCommand GoToAddTransferCommand =>
                new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Transfer }));
    }
}
