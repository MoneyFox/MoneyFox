using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GenericServices;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class RecurringPaymentViewModel : INotifyPropertyChanged, ILinkToEntity<RecurringPayment>
    {
        private int id;
        private int chargedAccountId;
        private int? targetAccountId;
        private int? categoryId;
        private DateTime startDate;
        private DateTime? endDate;
        private double amount;
        private bool isEndless;
        private PaymentType type;
        private PaymentRecurrence recurrence;
        private string note;

        private AccountViewModel chargedAccount;
        private AccountViewModel targetAccount;
        private CategoryViewModel categoryViewModel;

        public RecurringPaymentViewModel()
        {
            Recurrence = PaymentRecurrence.Daily;
            EndDate = DateTime.Today;
        }

        public int Id
        {
            get => id;
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
            get => chargedAccountId;
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
        public int? TargetAccountId
        {
            get => targetAccountId;
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
            get => categoryId;
            set
            {
                if (categoryId == value) return;
                categoryId = value;
                RaisePropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (startDate == value) return;
                startDate = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? EndDate
        {
            get => endDate;
            set
            {
                if (endDate == value) return;
                endDate = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEndless
        {
            get => isEndless;
            set
            {
                if (isEndless == value) return;
                isEndless = value;

                if (IsEndless)
                {
                    EndDate = null;
                }
                else
                {
                    EndDate = DateTime.Today;
                }
                
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Amount of the payment. Has to be >= 0. If the amount is charged or not is based on the payment type.
        /// </summary>
        public double Amount
        {
            get => amount;
            set
            {
                if (Math.Abs(amount - value) < 0.01) return;
                amount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Type of the payment <see cref="PaymentType" />.
        /// </summary>
        public PaymentType Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                RaisePropertyChanged();
            }
        }

        public PaymentRecurrence Recurrence
        {
            get => recurrence;
            set
            {
                if (recurrence == value) return;
                recurrence = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Additional notes to the payment.
        /// </summary>
        public string Note
        {
            get => note;
            set
            {
                if (note == value) return;
                note = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     In case it's a expense or transfer the account who will be charged.
        ///     In case it's an income the account who will be credited.
        /// </summary>
        public AccountViewModel ChargedAccount
        {
            get => chargedAccount;
            set
            {
                if(chargedAccount == value) return;
                chargedAccount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     The <see cref="AccountViewModel" /> who will be credited by a transfer.
        ///     Not used for the other payment types.
        /// </summary>
        public AccountViewModel TargetAccount
        {
            get => targetAccount;
            set
            {
                if (targetAccount == value) return;
                targetAccount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     The <see cref="Category" /> for this payment
        /// </summary>
        public CategoryViewModel Category
        {
            get => categoryViewModel;
            set
            {
                if (categoryViewModel == value) return;
                categoryViewModel = value;
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