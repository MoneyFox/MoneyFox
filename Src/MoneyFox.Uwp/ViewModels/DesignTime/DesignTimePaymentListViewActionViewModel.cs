using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimePaymentListViewActionViewModel : IPaymentListViewActionViewModel
    {
        public RelayCommand GoToAddIncomeCommand { get; } = null!;

        public RelayCommand GoToAddExpenseCommand { get; } = null!;

        public RelayCommand GoToAddTransferCommand { get; } = null!;

        public bool IsAddIncomeAvailable { get; }

        public bool IsAddExpenseAvailable { get; }

        public bool IsTransferAvailable { get; }

        public AsyncCommand DeleteAccountCommand { get; } = null!;

        public bool IsClearedFilterActive { get; set; }

        public bool IsRecurringFilterActive { get; set; }

        public DateTime TimeRangeStart { get; set; }

        public DateTime TimeRangeEnd { get; set; }
    }
}
