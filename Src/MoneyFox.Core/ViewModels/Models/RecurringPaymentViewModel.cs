using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MoneyFox.Core.DatabaseModels;

namespace MoneyFox.Core.ViewModels.Models
{
    /// <summary>
    ///     Provides and abstraction layer between the recurring payment database model and the use in the application.
    /// </summary>
    public class RecurringPaymentViewModel : INotifyPropertyChanged
    {
        private readonly RecurringPayment recurringPayment;

        public RecurringPaymentViewModel(RecurringPayment recurringPayment)
        {
            this.recurringPayment = recurringPayment;
        }

        public RecurringPaymentViewModel() : this(new RecurringPayment())
        {}

        public int Id => recurringPayment.Id;

        public Account ChargedAccount
        {
            get { return recurringPayment.ChargedAccount; }
            set { recurringPayment.ChargedAccount = value; }
        }

        public Account TargetAccount
        {
            get { return recurringPayment.ChargedAccount; }
            set { recurringPayment.ChargedAccount = value; }
        }

        public Category Category
        {
            get { return recurringPayment.Category; }
            set { recurringPayment.Category = value; }
        }

        public DateTime StartDate
        {
            get { return recurringPayment.StartDate; }
            set { recurringPayment.StartDate = value; }
        }

        public DateTime EndDate
        {
            get { return recurringPayment.EndDate; }
            set { recurringPayment.EndDate = value; }
        }

        public double Amount
        {
            get { return recurringPayment.Amount; }
            set { recurringPayment.Amount = value; }
        }

        public PaymentType Type
        {
            get { return (PaymentType) Enum.ToObject(typeof (PaymentType), recurringPayment.Type); }
            set { recurringPayment.Type = Convert.ToInt16(value); }
        }

        public string Note
        {
            get { return recurringPayment.Note; }
            set { recurringPayment.Note = value; }
        }
        public bool IsEndless
        {
            get { return recurringPayment.IsEndless; }
            set { recurringPayment.IsEndless = value; }
        }

        public PaymentRecurrence Recurrence
        {
            get { return (PaymentRecurrence) Enum.ToObject(typeof (PaymentRecurrence), recurringPayment.Recurrence); }
            set { recurringPayment.Recurrence = Convert.ToInt16(value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public RecurringPayment GetRecurringPayment() => recurringPayment;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}