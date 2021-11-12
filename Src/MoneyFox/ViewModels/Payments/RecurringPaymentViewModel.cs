using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Interfaces.Mapping;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.ViewModels.Accounts;
using MoneyFox.ViewModels.Categories;
using System;

namespace MoneyFox.ViewModels.Payments
{
    public class RecurringPaymentViewModel : ViewModelBase, IMapFrom<RecurringPayment>
    {
        private const decimal DECIMAL_DELTA = 0.01m;
        private decimal amount;
        private CategoryViewModel? categoryViewModel;

        private AccountViewModel chargedAccount = null!;
        private DateTime? endDate;

        private int id;
        private bool isEndless;
        private string note = "";
        private PaymentRecurrence recurrence;
        private DateTime startDate;
        private PaymentType type;

        public RecurringPaymentViewModel()
        {
            Recurrence = PaymentRecurrence.Daily;
            EndDate = null;
            IsEndless = true;
        }

        public int Id
        {
            get => id;
            set
            {
                if(id == value)
                {
                    return;
                }

                id = value;
                RaisePropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if(startDate == value)
                {
                    return;
                }

                startDate = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? EndDate
        {
            get => endDate;
            set
            {
                if(endDate == value)
                {
                    return;
                }

                endDate = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEndless
        {
            get => isEndless;
            set
            {
                if(isEndless == value)
                {
                    return;
                }

                isEndless = value;
                EndDate = isEndless is false
                    ? EndDate = DateTime.Today
                    : null;

                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Amount of the payment. Has to be >= 0. If the amount is charged or not is based on the payment type.
        /// </summary>
        public decimal Amount
        {
            get => amount;
            set
            {
                if(Math.Abs(amount - value) < DECIMAL_DELTA)
                {
                    return;
                }

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
                if(type == value)
                {
                    return;
                }

                type = value;
                RaisePropertyChanged();
            }
        }

        public PaymentRecurrence Recurrence
        {
            get => recurrence;
            set
            {
                if(recurrence == value)
                {
                    return;
                }

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
                if(note == value)
                {
                    return;
                }

                note = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     In case it's a expense or transfer the account who will be charged.     In case it's an income the account
        ///     who will be credited.
        /// </summary>
        public AccountViewModel ChargedAccount
        {
            get => chargedAccount;
            set
            {
                if(chargedAccount == value)
                {
                    return;
                }

                chargedAccount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     The <see cref="Category" /> for this payment
        /// </summary>
        public CategoryViewModel? Category
        {
            get => categoryViewModel;
            set
            {
                if(categoryViewModel == value)
                {
                    return;
                }

                categoryViewModel = value;
                RaisePropertyChanged();
            }
        }
    }
}