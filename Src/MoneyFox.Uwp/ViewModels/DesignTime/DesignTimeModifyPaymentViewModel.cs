using GalaSoft.MvvmLight.Command;
using MoneyFox.Domain;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeModifyPaymentViewModel : IModifyPaymentViewModel
    {
        public bool IsTransfer { get; }

        public DateTime EndDate { get; } = DateTime.Now;

        public PaymentRecurrence Recurrence { get; } = PaymentRecurrence.Monthly;

        public List<PaymentRecurrence> RecurrenceList
                                       => new List<PaymentRecurrence>
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

        public PaymentViewModel SelectedPayment { get; } = null!;

        public string AmountString { get; } = "";

        public ObservableCollection<AccountViewModel> ChargedAccounts { get; } = null!;

        public ObservableCollection<AccountViewModel> TargetAccounts { get; } = null!;

        public string Title { get; } = "My Title";

        public string AccountHeader { get; } = "";

        public DateTime Date { get; }

        public RelayCommand SelectedItemChangedCommand { get; } = null!;

        public AsyncCommand SaveCommand { get; } = null!;

        public RelayCommand GoToSelectCategoryDialogCommand { get; } = null!;

        public RelayCommand DeleteCommand { get; } = null!;

        public RelayCommand CancelCommand { get; } = null!;

        public RelayCommand ResetCategoryCommand { get; } = null!;
    }
}
