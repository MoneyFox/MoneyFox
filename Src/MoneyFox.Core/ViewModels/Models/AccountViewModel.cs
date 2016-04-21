using MoneyFox.Core.DatabaseModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoneyFox.Core.ViewModels.Models
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private Account account;

        public AccountViewModel() 
            : this(new Account()) { }

        public AccountViewModel(Account account)
        {
            this.account = account;
        }

        public int Id => account.Id;

        public string Name
        {
            get { return account.Name; }
            set { account.Name = value; }
        }

        public double CurrentBalance
        {
            get { return account.CurrentBalance; }
            set { account.CurrentBalance = value; }
        }

        public string Iban
        {
            get { return account.Iban; }
            set { account.Iban = value; }
        }

        public string Note
        {
            get { return account.Note; }
            set { account.Note = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
