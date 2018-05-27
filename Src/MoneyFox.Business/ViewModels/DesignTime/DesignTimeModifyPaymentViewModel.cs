using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoneyFox.Foundation;
using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeModifyPaymentViewModel : BaseViewModel, IModifyPaymentViewModel
    {
        public bool IsEdit { get; } = false;
        public bool IsTransfer { get; } = false;
        public bool IsEndless { get; } = true;
        public DateTime EndDate { get; } = DateTime.Now;
        public PaymentRecurrence Recurrence { get; } = PaymentRecurrence.Monthly;
        public string AmountString { get; } = "451,95";
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

        public PaymentViewModel SelectedPayment { get; } = null;

        public ObservableCollection<AccountViewModel> ChargedAccounts { get; }

        public ObservableCollection<AccountViewModel> TargetAccounts { get; }

        public string Title { get; } = "My Title";
        public string AccountHeader { get; }
        public DateTime Date { get; }
        public int PaymentId { get; }

        public IMvxCommand SelectedItemChangedCommand { get; } = null;
        public IMvxAsyncCommand SaveCommand { get; } = null;
        public IMvxAsyncCommand GoToSelectCategorydialogCommand { get; } = null;
        public IMvxAsyncCommand DeleteCommand { get; } = null;
        public IMvxAsyncCommand CancelCommand { get; } = null;
        public IMvxCommand ResetCategoryCommand { get; } = null;
    }
}
