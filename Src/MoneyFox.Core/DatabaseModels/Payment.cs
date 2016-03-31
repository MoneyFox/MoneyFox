using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MoneyFox.Core.DatabaseModels
{
    /// <summary>
    ///     Databasemodel for payments. Includes expenses, income and transfers.
    ///     Databasetable: Payments
    /// </summary>
    [Table("Payments")]
    public class Payment : INotifyPropertyChanged
    {
        private Category category;

        private Account chargedAccount;

        private RecurringPayment recurringPayment;

        private Account targetAccount;

        public int Id { get; set; }

        public int ChargedAccountId { get; set; }

        public int TargetAccountId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public bool IsCleared { get; set; }

        public int Type { get; set; }

        public string Note { get; set; }

        public bool IsRecurring { get; set; }

        public int RecurringPaymentId { get; set; }

        [NotMapped]
        public Account ChargedAccount
        {
            get { return chargedAccount; }
            set
            {
                if (chargedAccount != value)
                {
                    chargedAccount = value;
                    ChargedAccountId = value.Id;
                }
            }
        }

        [NotMapped]
        public Account TargetAccount
        {
            get { return targetAccount; }
            set
            {
                if (targetAccount != value)
                {
                    targetAccount = value;
                    TargetAccountId = value.Id;
                }
            }
        }

        [NotMapped]
        public Category Category
        {
            get { return category; }
            set
            {
                if (category != value)
                {
                    category = value;
                    CategoryId = value?.Id;
                    OnPropertyChanged();
                }
            }
        }

        [NotMapped]
        public RecurringPayment RecurringPayment
        {
            get { return recurringPayment; }
            set
            {
                if (recurringPayment != value)
                {
                    recurringPayment = value;
                    RecurringPaymentId = value.Id;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}