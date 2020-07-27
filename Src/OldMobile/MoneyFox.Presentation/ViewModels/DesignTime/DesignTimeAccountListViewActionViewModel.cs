using GalaSoft.MvvmLight.Command;
using MoneyFox.Presentation.ViewModels.Interfaces;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewActionViewModel : IAccountListViewActionViewModel
    {
        public RelayCommand GoToAddIncomeCommand { get; }

        public RelayCommand GoToAddExpenseCommand { get; }

        public RelayCommand GoToAddTransferCommand { get; }

        public bool IsAddIncomeAvailable { get; }

        public bool IsAddExpenseAvailable { get; }

        public bool IsTransferAvailable { get; }

        public RelayCommand GoToAddAccountCommand { get; }
    }
}
