using System;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Presentation.ViewModels.Interfaces;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimePaymentListViewActionViewModel : IPaymentListViewActionViewModel
    {
        public RelayCommand GoToAddIncomeCommand { get; }
        public RelayCommand GoToAddExpenseCommand { get; }
        public RelayCommand GoToAddTransferCommand { get; }
        public bool IsAddIncomeAvailable { get; }
        public bool IsAddExpenseAvailable { get; }
        public bool IsTransferAvailable { get; }
        public AsyncCommand DeleteAccountCommand { get; }
        public bool IsClearedFilterActive { get; set; }
        public bool IsRecurringFilterActive { get; set; }
        public DateTime TimeRangeStart { get; set; }
        public DateTime TimeRangeEnd { get; set; }
    }
}
