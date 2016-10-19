using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MoneyFox.Shared.ViewModels.Models;

namespace MoneyFox.Shared.Model
{
    public class PaymentViewModel : INotifyPropertyChanged
    {
        private CategoryViewModel category;

        private AccountViewModel chargedAccount;

        private RecurringPaymentViewModel recurringPayment;

        private AccountViewModel targetAccount;

        private int id;
        private int chargedAccountId;
        private int targetAccountId;
        private int? categoryId;
        private DateTime date;
        private double amount;
        private bool isCleared;
        private int type;
        private string note;
        private bool isRecurring;
        private int recurringPaymentId;
        private int currentAccountId;

        public int Id
        {
            get { return id; }
            set
            {
                if (id == value) return;
                id = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     In case it's a expense or transfer the foreign key to the <see cref="AccountViewModel" /> who will be charged.
        ///     In case it's an income the  foreign key to the <see cref="AccountViewModel" /> who will be credited.
        /// </summary>
        public int ChargedAccountId
        {
            get { return chargedAccountId; }
            set
            {
                if (chargedAccountId == value) return;
                chargedAccountId = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Foreign key to the account who will be credited by a transfer.
        ///     Not used for the other payment types.
        /// </summary>
        public int TargetAccountId
        {
            get { return targetAccountId; }
            set
            {
                if (targetAccountId == value) return;
                targetAccountId = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Foreign key to the <see cref="Category" /> for this payment
        /// </summary>
        public int? CategoryId
        {
            get { return categoryId; }
            set
            {
                if (categoryId == value) return;
                categoryId = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Date when this payment will be executed.
        /// </summary>
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date == value) return;
                date = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Amount of the payment. Has to be >= 0. If the amount is charged or not is based on the payment type.
        /// </summary>
        public double Amount
        {
            get { return amount; }
            set
            {
                if (Math.Abs(amount - value) < 0.01) return;
                amount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Indicates if this payment was already executed and the amount already credited or charged to the respective
        ///     account.
        /// </summary>
        public bool IsCleared
        {
            get { return isCleared; }
            set
            {
                if (isCleared == value) return;
                isCleared = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Type of the payment. This is the int of the Enum <see cref="PaymentType" />.
        /// </summary>
        public int Type
        {
            get { return type; }
            set
            {
                if (type == value) return;
                type = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Additional notes to the payment.
        /// </summary>
        public string Note
        {
            get { return note; }
            set
            {
                if (note == value) return;
                note = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Indicates if the payment will be repeated or if it's a uniquie payment.
        /// </summary>
        public bool IsRecurring
        {
            get { return isRecurring; }
            set
            {
                if (isRecurring == value) return;
                isRecurring = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Foreign key to the <see cref="RecurringPayment" /> if it's recurring.
        /// </summary>
        public int RecurringPaymentId
        {
            get { return recurringPaymentId; }
            set
            {
                if (recurringPaymentId == value) return;
                recurringPaymentId = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     In case it's a expense or transfer the account who will be charged.
        ///     In case it's an income the account who will be credited.
        /// </summary>
        public AccountViewModel ChargedAccount
        {
            get { return chargedAccount; }
            set
            {
                if (chargedAccount != value)
                {
                    chargedAccount = value;
                    ChargedAccountId = value?.Id ?? 0;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        ///     The <see cref="AccountViewModel" /> who will be credited by a transfer.
        ///     Not used for the other payment types.
        /// </summary>
        public AccountViewModel TargetAccount
        {
            get { return targetAccount; }
            set
            {
                if (targetAccount != value)
                {
                    targetAccount = value;
                    TargetAccountId = value?.Id ?? 0;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        ///     The <see cref="Category" /> for this payment
        /// </summary>
        public CategoryViewModel Category
        {
            get { return category; }
            set
            {
                if (category != value)
                {
                    category = value;
                    CategoryId = value?.Id ?? 0;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        ///     The <see cref="RecurringPayment" /> if it's recurring.
        /// </summary>
        public RecurringPaymentViewModel RecurringPayment
        {
            get { return recurringPayment; }
            set
            {
                if (recurringPayment != value)
                {
                    recurringPayment = value;
                    RecurringPaymentId = value?.Id ?? 0;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        ///     Checks if the payment is ready to clear based on the date of
        ///     the payment and the current date.
        /// </summary>
        public bool ClearPaymentNow => Date.Date <= DateTime.Now.Date;

        /// <summary>
        ///     This is a shortcut to access if the payment is a transfer or not.
        /// </summary>
        public bool IsTransfer => Type == (int) PaymentType.Transfer;

        public int CurrentAccountId
        {
            get { return currentAccountId; }
            set
            {
                if (currentAccountId == value) return;
                currentAccountId = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}