using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MoneyFox.Core.DatabaseModels;

namespace MoneyFox.Core.ViewModels.Models
{
    /// <summary>
    ///     Provides and abstraction layer between the payment database model and the use in the application.
    /// </summary>
    public class PaymentViewModel : INotifyPropertyChanged
    {
        private readonly Payment payment;

        public PaymentViewModel() 
            : this(new Payment())
        {}

        public PaymentViewModel(Payment payment)
        {
            this.payment = payment;
        }

        /// <summary>
        ///     Returns the id of the underlying payment. This will only be set through the database.
        /// </summary>
        public int Id => payment.Id;

        public Account ChargedAccount
        {
            get { return payment.ChargedAccount; }
            set { payment.ChargedAccount = value; }
        }

        public Account TargetAccount
        {
            get { return payment.ChargedAccount; }
            set { payment.ChargedAccount = value; }
        }

        public Category Category
        {
            get { return payment.Category; }
            set { payment.Category = value; }
        }

        public DateTime Date
        {
            get { return payment.Date; }
            set { payment.Date = value; }
        }

        public double Amount
        {
            get { return payment.Amount; }
            set { payment.Amount = value; }
        }

        public PaymentType Type
        {
            get { return (PaymentType) Enum.ToObject(typeof (PaymentType), payment.Type); }
            set { payment.Type = Convert.ToInt16(value); }
        }

        public string Note
        {
            get { return payment.Note; }
            set { payment.Note = value; }
        }

        public bool IsCleared
        {
            get { return payment.IsCleared; }
            set { payment.IsCleared = value; }
        }

        public bool IsRecurring
        {
            get { return payment.IsRecurring; }
            set { payment.IsRecurring = value; }
        }

        public RecurringPayment RecurringPayment
        {
            get { return payment.RecurringPayment; }
            set { payment.RecurringPayment = value; }
        }

        /// <summary>
        ///     Checks if the payment is ready to clear or not.
        ///     It is if the Date is today or already passed.
        /// </summary>
        public bool ClearPaymentNow => Date.Date <= DateTime.Now.Date;

        /// <summary>
        ///     Checks if the payment is a transfer
        /// </summary>
        public bool IsTransfer => Type == PaymentType.Transfer;

        /// <summary>
        ///     Returns the underlying payment.
        ///     This should not be used to make modifications to the payment object!
        /// </summary>
        /// <returns></returns>
        public Payment GetPayment() => payment;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}