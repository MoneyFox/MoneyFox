using GenericServices;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;
using ReactiveUI;
using System;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Handles the view representation of a payment.
    /// </summary>
    public class PaymentViewModel : ViewModelBase, ILinkToEntity<Payment>
    {
        private int id;
        private int chargedAccountId;
        private int? targetAccountId;
        private DateTime date;
        private double amount;
        private bool isCleared;
        private PaymentType type;
        private string note;
        private bool isRecurring;

        private AccountViewModel chargedAccount;
        private AccountViewModel targetAccount;
        private CategoryViewModel categoryViewModel;
        private RecurringPaymentViewModel recurringPaymentViewModel;

        public PaymentViewModel()
        {
            Date = DateTime.Today;
            Note = string.Empty;
        }

        public int Id
        {
            get => id;
            set => this.RaiseAndSetIfChanged(ref id, value);
        }

        /// <summary>
        ///     In case it's a expense or transfer the foreign key to the <see cref="AccountViewModel" /> who will be charged.
        ///     In case it's an income the  foreign key to the <see cref="AccountViewModel" /> who will be credited.
        /// </summary>
        public int ChargedAccountId
        {
            get => chargedAccountId;
            set => this.RaiseAndSetIfChanged(ref chargedAccountId, value);
        }

        /// <summary>
        ///     Foreign key to the account who will be credited by a transfer.
        ///     Not used for the other payment types.
        /// </summary>
        public int? TargetAccountId
        {
            get => targetAccountId;
            set => this.RaiseAndSetIfChanged(ref targetAccountId, value);
        }

        /// <summary>
        ///     Date when this payment will be executed.
        /// </summary>
        public DateTime Date
        {
            get => date;
            set => this.RaiseAndSetIfChanged(ref date, value);
        }

        /// <summary>
        ///     Amount of the payment. Has to be >= 0. If the amount is charged or not is based on the payment type.
        /// </summary>
        public double Amount
        {
            get => amount;
            set => this.RaiseAndSetIfChanged(ref amount, value);
        }

        /// <summary>
        ///     Indicates if this payment was already executed and the amount already credited or charged to the respective
        ///     account.
        /// </summary>
        public bool IsCleared
        {
            get => isCleared;
            set => this.RaiseAndSetIfChanged(ref isCleared, value);
        }

        /// <summary>
        ///     Type of the payment <see cref="PaymentType" />.
        /// </summary>
        public PaymentType Type
        {
            get => type;
            set => this.RaiseAndSetIfChanged(ref type, value);
        }

        /// <summary>
        ///     Additional notes to the payment.
        /// </summary>
        public string Note
        {
            get => note;
            set => this.RaiseAndSetIfChanged(ref note, value);
        }

        /// <summary>
        ///     Indicates if the payment will be repeated or if it's a uniquie payment.
        /// </summary>
        public bool IsRecurring
        {
            get => isRecurring;
            set
            {
                this.RaiseAndSetIfChanged(ref isRecurring, value);
                RecurringPayment = isRecurring
                    ? new RecurringPaymentViewModel()
                    : null;
            }
        }

        /// <summary>
        ///     In case it's a expense or transfer the account who will be charged.
        ///     In case it's an income the account who will be credited.
        /// </summary>
        public AccountViewModel ChargedAccount
        {
            get => chargedAccount;
            set => this.RaiseAndSetIfChanged(ref chargedAccount, value);
        }

        /// <summary>
        ///     The <see cref="AccountViewModel" /> who will be credited by a transfer.
        ///     Not used for the other payment types.
        /// </summary>
        public AccountViewModel TargetAccount
        {
            get => targetAccount;
            set => this.RaiseAndSetIfChanged(ref targetAccount, value);
        }

        /// <summary>
        ///     The <see cref="Category" /> for this payment
        /// </summary>
        public CategoryViewModel Category
        {
            get => categoryViewModel;
            set => this.RaiseAndSetIfChanged(ref categoryViewModel, value);
        }

        /// <summary>
        ///     The <see cref="RecurringPayment" /> if it's recurring.
        /// </summary>
        public RecurringPaymentViewModel RecurringPayment
        {
            get => recurringPaymentViewModel;
            set => this.RaiseAndSetIfChanged(ref recurringPaymentViewModel, value);
        }

        /// <summary>
        ///     This is a shortcut to access if the payment is a transfer or not.
        /// </summary>
        public bool IsTransfer => Type == PaymentType.Transfer;

        private int currentAccountId;

        /// <summary>
        ///     Id of the account who currently is used for that view.
        /// </summary>
        public int CurrentAccountId
        {
            get => currentAccountId;
            set => this.RaiseAndSetIfChanged(ref currentAccountId, value);
        }
    }
}