using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Interfaces.Mapping;
using MoneyFox.Domain.Entities;
using System;

namespace MoneyFox.Uwp.ViewModels
{
    /// <summary>
    /// Representation of an account view.
    /// </summary>
    public class AccountViewModel : ViewModelBase, IMapFrom<Account>
    {
        private int id;
        private string name;
        private decimal currentBalance;
        private string note;
        private bool isOverdrawn;
        private bool isExcluded;

        /// <summary>
        /// Account Id
        /// </summary>
        public int Id
        {
            get => id;
            set
            {
                if(id == value)
                    return;
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
                    return;
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
                if(Math.Abs(currentBalance - value) < 0.01m)
                    return;
                currentBalance = value;
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
                    return;
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
                    return;
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
                    return;
                isExcluded = value;
                RaisePropertyChanged();
            }
        }

        public override bool Equals(object obj)
        {
            return (obj as AccountViewModel)?.Id == Id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 7;
                hashCode = (hashCode * 397) ^ id.GetHashCode();

                return hashCode;
            }
        }
    }
}
