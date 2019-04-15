using System;
using GenericServices;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Representation of an account view.
    /// </summary>
    public class AccountViewModel : BaseViewModel, ILinkToEntity<Account>
    {
        private int id;
        private string name;
        private double currentBalance;
        private string note;
        private bool isOverdrawn;
        private bool isExcluded;

        /// <summary>
        ///     Account Id
        /// </summary>
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
        ///     Account Name
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Current Balance
        /// </summary>
        public double CurrentBalance
        {
            get => currentBalance;
            set
            {
                if (Math.Abs(currentBalance - value) < 0.01) return;
                currentBalance = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Note
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
        ///     Indicator if the account currently is overdrawn.
        /// </summary>
        public bool IsOverdrawn
        {
            get => isOverdrawn;
            set
            {
                if (isOverdrawn == value) return;
                isOverdrawn = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Indicator if the account is excluded from the balance calculation.
        /// </summary>
        public bool IsExcluded
        {
            get => isExcluded;
            set
            {
                if (isExcluded == value) return;
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
                var hashCode = id;
                hashCode = (hashCode * 397) ^ (name != null ? name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ currentBalance.GetHashCode();
                hashCode = (hashCode * 397) ^ (note != null ? note.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ isOverdrawn.GetHashCode();
                hashCode = (hashCode * 397) ^ isExcluded.GetHashCode();
                return hashCode;
            }
        }
    }
}