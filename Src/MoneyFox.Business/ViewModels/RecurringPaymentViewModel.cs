using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.Business.ViewModels
{
    public class RecurringPaymentViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     Default constructor. Will create a new <see cref="RecurringPayment" />
        /// </summary>
        public RecurringPaymentViewModel()
        {
            RecurringPayment = new RecurringPayment();
        }

        /// <summary>
        ///     Constructor. Assignes the passed recurring payment to the wrapped payment field.
        /// </summary>
        /// <param name="recurringPayment">RecurringPayment to use for further operations.</param>
        public RecurringPaymentViewModel(RecurringPayment recurringPayment)
        {
            RecurringPayment = recurringPayment;
        }

        public RecurringPayment RecurringPayment { get; set; }

        public int Id
        {
            get => RecurringPayment.Data.Id;
            set
            {
                if (RecurringPayment.Data.Id == value) return;
                RecurringPayment.Data.Id = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     In case it's a expense or transfer the foreign key to the <see cref="AccountViewModel" /> who will be charged.
        ///     In case it's an income the  foreign key to the <see cref="AccountViewModel" /> who will be credited.
        /// </summary>
        public int ChargedAccountId
        {
            get => RecurringPayment.Data.ChargedAccountId;
            set
            {
                if (RecurringPayment.Data.ChargedAccountId == value) return;
                RecurringPayment.Data.ChargedAccountId = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Foreign key to the account who will be credited by a transfer.
        ///     Not used for the other payment types.
        /// </summary>
        public int? TargetAccountId
        {
            get => RecurringPayment.Data.TargetAccountId;
            set
            {
                if (RecurringPayment.Data.TargetAccountId == value) return;
                RecurringPayment.Data.TargetAccountId = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Foreign key to the <see cref="Category" /> for this payment
        /// </summary>
        public int? CategoryId
        {
            get => RecurringPayment.Data.CategoryId;
            set
            {
                if (RecurringPayment.Data.CategoryId == value) return;
                RecurringPayment.Data.CategoryId = value;
                RaisePropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => RecurringPayment.Data.StartDate;
            set
            {
                if (RecurringPayment.Data.StartDate == value) return;
                RecurringPayment.Data.StartDate = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? EndDate
        {
            get => RecurringPayment.Data.EndDate;
            set
            {
                if (RecurringPayment.Data.EndDate == value) return;
                RecurringPayment.Data.EndDate = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEndless
        {
            get => RecurringPayment.Data.IsEndless;
            set
            {
                if (RecurringPayment.Data.IsEndless == value) return;
                RecurringPayment.Data.IsEndless = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Amount of the payment. Has to be >= 0. If the amount is charged or not is based on the payment type.
        /// </summary>
        public double Amount
        {
            get => RecurringPayment.Data.Amount;
            set
            {
                if (Math.Abs(RecurringPayment.Data.Amount - value) < 0.01) return;
                RecurringPayment.Data.Amount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Type of the payment <see cref="PaymentType" />.
        /// </summary>
        public PaymentType Type
        {
            get => RecurringPayment.Data.Type;
            set
            {
                if (RecurringPayment.Data.Type == value) return;
                RecurringPayment.Data.Type = value;
                RaisePropertyChanged();
            }
        }

        public PaymentRecurrence Recurrence
        {
            get => RecurringPayment.Data.Recurrence;
            set
            {
                if (RecurringPayment.Data.Recurrence == value) return;
                RecurringPayment.Data.Recurrence = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Additional notes to the payment.
        /// </summary>
        public string Note
        {
            get => RecurringPayment.Data.Note;
            set
            {
                if (RecurringPayment.Data.Note == value) return;
                RecurringPayment.Data.Note = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     In case it's a expense or transfer the account who will be charged.
        ///     In case it's an income the account who will be credited.
        /// </summary>
        public AccountViewModel ChargedAccount
        {
            get => RecurringPayment.Data.ChargedAccount != null
                ? new AccountViewModel(new Account(RecurringPayment.Data.ChargedAccount))
                : null;
            set
            {
                RecurringPayment.Data.ChargedAccount = value.Account.Data;
                ChargedAccount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     The <see cref="AccountViewModel" /> who will be credited by a transfer.
        ///     Not used for the other payment types.
        /// </summary>
        public AccountViewModel TargetAccount
        {
            get => RecurringPayment.Data.TargetAccount != null
                ? new AccountViewModel(new Account(RecurringPayment.Data.TargetAccount))
                : null;
            set
            {
                RecurringPayment.Data.TargetAccount = value.Account.Data;
                TargetAccount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     The <see cref="Category" /> for this payment
        /// </summary>
        public CategoryViewModel Category
        {
            get => RecurringPayment.Data.Category != null
                ? new CategoryViewModel(new Category(RecurringPayment.Data.Category))
                : null;
            set
            {
                RecurringPayment.Data.Category = value.Category.Data;
                Category = value;
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