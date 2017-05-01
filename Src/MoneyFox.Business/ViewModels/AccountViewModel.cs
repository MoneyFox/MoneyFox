using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Business.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        public AccountViewModel(Account account)
        {
            Account = account;
        }

        public Account Account { get; }

        public int Id
        {
            get { return Account.Data.Id; }
            set
            {
                if (Account.Data.Id == value) return;
                Account.Data.Id = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get { return Account.Data.Name; }
            set
            {
                if (Account.Data.Name == value) return;
                Account.Data.Name = value;
                RaisePropertyChanged();
            }
        }

        public string Iban
        {
            get { return Account.Data.Iban; }
            set
            {
                if (Account.Data.Iban == value) return;
                Account.Data.Iban = value;
                RaisePropertyChanged();
            }
        }

        public double CurrentBalance
        {
            get { return Account.Data.CurrentBalance; }
            set
            {
                if (Math.Abs(Account.Data.CurrentBalance - value) < 0.01) return;
                Account.Data.CurrentBalance = value;
                RaisePropertyChanged();
            }
        }

        public string Note
        {
            get { return Account.Data.Note; }
            set
            {
                if (Account.Data.Note == value) return;
                Account.Data.Note = value;
                RaisePropertyChanged();
            }
        }

        public bool IsOverdrawn
        {
            get { return Account.Data.IsOverdrawn; }
            set
            {
                if (Account.Data.IsOverdrawn == value) return;
                Account.Data.IsOverdrawn = value;
                RaisePropertyChanged();
            }
        }

        public bool IsExcluded
        {
            get { return Account.Data.IsExcluded; }
            set
            {
                if (Account.Data.IsExcluded == value) return;
                Account.Data.IsExcluded = value;
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