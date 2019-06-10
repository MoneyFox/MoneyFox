using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Commands;

namespace MoneyFox.ServiceLayer.ViewModels.DesignTime
{
    public class DesignTimeModifyPaymentViewModel : IModifyPaymentViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public bool IsTransfer { get; } = false;
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

        public PaymentViewModel SelectedPayment { get; } = null;
        public string AmountString { get; }

        public ObservableCollection<AccountViewModel> ChargedAccounts { get; }

        public ObservableCollection<AccountViewModel> TargetAccounts { get; }

        public string Title { get; } = "My Title";
        public string AccountHeader { get; }
        public DateTime Date { get; }

        public IMvxCommand SelectedItemChangedCommand { get; } = null;
        public IMvxAsyncCommand SaveCommand { get; } = null;
        public IMvxAsyncCommand GoToSelectCategorydialogCommand { get; } = null;
        public IMvxAsyncCommand DeleteCommand { get; } = null;
        public IMvxAsyncCommand CancelCommand { get; } = null;
        public IMvxCommand ResetCategoryCommand { get; } = null;
    }
}
