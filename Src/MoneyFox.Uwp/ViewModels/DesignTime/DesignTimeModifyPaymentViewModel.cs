using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Domain;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeModifyPaymentViewModel : IModifyPaymentViewModel
    {
        public bool IsTransfer { get; }
        public DateTime EndDate { get; } = DateTime.Now;
        public PaymentRecurrence Recurrence { get; } = PaymentRecurrence.Monthly;

        public List<PaymentRecurrence> RecurrenceList => new List<PaymentRecurrence>
        {
            PaymentRecurrence.Daily,
            PaymentRecurrence.DailyWithoutWeekend,
            PaymentRecurrence.Weekly,
            PaymentRecurrence.Biweekly,
            PaymentRecurrence.Monthly,
            PaymentRecurrence.Bimonthly,
            PaymentRecurrence.Quarterly,
            PaymentRecurrence.Biannually,
            PaymentRecurrence.Yearly
        };

        public PaymentViewModel SelectedPayment { get; }
        public string AmountString { get; }

        public ObservableCollection<AccountViewModel> ChargedAccounts { get; }

        public ObservableCollection<AccountViewModel> TargetAccounts { get; }

        public string Title { get; } = "My Title";
        public string AccountHeader { get; }
        public DateTime Date { get; }

        public RelayCommand SelectedItemChangedCommand { get; }
        public AsyncCommand SaveCommand { get; }
        public RelayCommand GoToSelectCategoryDialogCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand ResetCategoryCommand { get; }
    }
}
