using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.Presentation.ViewModels.DesignTime
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

        public RelayCommand SelectedItemChangedCommand { get; } = null;
        public RelayCommand SaveCommand { get; } = null;
        public RelayCommand GoToSelectCategorydialogCommand { get; } = null;
        public RelayCommand DeleteCommand { get; } = null;
        public RelayCommand CancelCommand { get; } = null;
        public RelayCommand ResetCategoryCommand { get; } = null;
    }
}
