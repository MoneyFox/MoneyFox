using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Interfaces.Mapping;
using MoneyFox.Domain.Entities;
using System;

namespace MoneyFox.Ui.Shared.ViewModels.Accounts
{
    public sealed class AccountViewModel : ViewModelBase, IMapFrom<Account>, IEquatable<AccountViewModel>
    {
        private const decimal DECIMAL_DELTA = 0.01m;

        private int id;
        private string name = "";
        private decimal currentBalance;
        private decimal endOfMonthBalance;
        private string note = "";
        private bool isOverdrawn;
        private bool isExcluded;
        private DateTime creationTime;
        private DateTime modificationDate;

        /// <summary>
        /// Account Id
        /// </summary>
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

        /// <summary>
        /// Account Name
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if(name == value)
                {
                    return;
                }

                name = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Current Balance
        /// </summary>
        public decimal CurrentBalance
        {
            get => currentBalance;
            set
            {
                if(Math.Abs(currentBalance - value) < DECIMAL_DELTA)
                {
                    return;
                }

                currentBalance = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Balance End of Month
        /// </summary>
        public decimal EndOfMonthBalance
        {
            get => endOfMonthBalance;
            set
            {
                endOfMonthBalance = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Note
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
        /// Indicator if the account currently is overdrawn.
        /// </summary>
        public bool IsOverdrawn
        {
            get => isOverdrawn;
            set
            {
                if(isOverdrawn == value)
                {
                    return;
                }

                isOverdrawn = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Indicator if the account is excluded from the balance calculation.
        /// </summary>
        public bool IsExcluded
        {
            get => isExcluded;
            set
            {
                if(isExcluded == value)
                {
                    return;
                }

                isExcluded = value;
                RaisePropertyChanged();
            }
        }

        public DateTime CreationTime
        {
            get => creationTime;
            set
            {
                if(creationTime == value)
                {
                    return;
                }

                creationTime = value;
                RaisePropertyChanged();
            }
        }

        public DateTime ModificationDate
        {
            get => modificationDate;
            set
            {
                if(modificationDate == value)
                {
                    return;
                }

                modificationDate = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(AccountViewModel other)
        {
            if(other == null)
            {
                return false;
            }
            return Id.Equals(other.Id);
        }
    }
}
