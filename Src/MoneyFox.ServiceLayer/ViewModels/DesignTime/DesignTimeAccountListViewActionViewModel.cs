using System.Globalization;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Commands;

namespace MoneyFox.ServiceLayer.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewActionViewModel : IAccountListViewActionViewModel
    {
        public DesignTimeAccountListViewActionViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <inheritdoc />
        public LocalizedResources Resources { get; }

        public MvxAsyncCommand GoToAddIncomeCommand { get; }
        public MvxAsyncCommand GoToAddExpenseCommand { get; }
        public MvxAsyncCommand GoToAddTransferCommand { get; }
        public bool IsAddIncomeAvailable { get; }
        public bool IsAddExpenseAvailable { get; }
        public bool IsTransferAvailable { get; }
        public MvxAsyncCommand GoToAddAccountCommand { get; }
    }
}
