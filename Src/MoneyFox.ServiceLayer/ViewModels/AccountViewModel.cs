using System;
using MoneyFox.DataAccess.Pocos;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Representation of an account view.
    /// </summary>
    public class AccountViewModel : BaseViewModel
    {
        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="account">Account wrap</param>
        public AccountViewModel(Account account)
        {
            Account = account;
        }

        /// <summary>
        ///     Account Data
        /// </summary>
        public Account Account { get; }

        /// <summary>
        ///     Account Id
        /// </summary>
        public int Id
        {
            get => Account.Data.Id;
            set
            {
                if (Account.Data.Id == value) return;
                Account.Data.Id = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Account Name
        /// </summary>
        public string Name
        {
            get => Account.Data.Name;
            set
            {
                if (Account.Data.Name == value) return;
                Account.Data.Name = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Account IBAN number or account number.
        /// </summary>
        public string Iban
        {
            get => Account.Data.Iban;
            set
            {
                if (Account.Data.Iban == value) return;
                Account.Data.Iban = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Current Balance
        /// </summary>
        public double CurrentBalance
        {
            get => Account.Data.CurrentBalance;
            set
            {
                if (Math.Abs(Account.Data.CurrentBalance - value) < 0.01) return;
                Account.Data.CurrentBalance = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Note
        /// </summary>
        public string Note
        {
            get => Account.Data.Note;
            set
            {
                if (Account.Data.Note == value) return;
                Account.Data.Note = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Indicator if the account currently is overdrawn.
        /// </summary>
        public bool IsOverdrawn
        {
            get => Account.Data.IsOverdrawn;
            set
            {
                if (Account.Data.IsOverdrawn == value) return;
                Account.Data.IsOverdrawn = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Indicator if the account is excluded from the balance calculation.
        /// </summary>
        public bool IsExcluded
        {
            get => Account.Data.IsExcluded;
            set
            {
                if (Account.Data.IsExcluded == value) return;
                Account.Data.IsExcluded = value;
                RaisePropertyChanged();
            }
        }
    }
}