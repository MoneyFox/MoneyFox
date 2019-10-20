using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Utilities;

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
