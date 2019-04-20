using GenericServices;
using MoneyFox.DataLayer.Entities;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Representation of an account view.
    /// </summary>
    public class AccountViewModel : ViewModelBase, ILinkToEntity<Account>
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
            set => this.RaiseAndSetIfChanged(ref id, value);
        }

        /// <summary>
        ///     Account Name
        /// </summary>
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        /// <summary>
        ///     Current Balance
        /// </summary>
        public double CurrentBalance
        {
            get => currentBalance;
            set => this.RaiseAndSetIfChanged(ref currentBalance, value);
        }

        /// <summary>
        ///     Note
        /// </summary>
        public string Note
        {
            get => note;
            set => this.RaiseAndSetIfChanged(ref note, value);
        }

        /// <summary>
        ///     Indicator if the account currently is overdrawn.
        /// </summary>
        public bool IsOverdrawn
        {
            get => isOverdrawn;
            set => this.RaiseAndSetIfChanged(ref isOverdrawn, value);
        }

        /// <summary>
        ///     Indicator if the account is excluded from the balance calculation.
        /// </summary>
        public bool IsExcluded
        {
            get => isExcluded;
            set => this.RaiseAndSetIfChanged(ref isExcluded, value);
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