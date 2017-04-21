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
            Data = account;
        }

        public Account Data { get; private set; }

        public int Id
        {
            get { return Data.Data.Id; }
            set
            {
                if (Data.Data.Id == value) return;
                Data.Data.Id = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get { return Data.Data.Name; }
            set
            {
                if (Data.Data == value) return;
                Data.Data = value;
                RaisePropertyChanged();
            }
        }

        public string Iban
        {
            get { return Data.Data.Iban; }
            set
            {
                if (Data.Data.Iban == value) return;
                Data.Data.Iban = value;
                RaisePropertyChanged();
            }
        }

        public double CurrentBalance
        {
            get { return Data.Data.CurrentBalance; }
            set
            {
                if (Math.Abs(Data.Data.CurrentBalance - value) < 0.01) return;
                Data.Data.CurrentBalance = value;
                RaisePropertyChanged();
            }
        }

        public string Note
        {
            get { return Data.Data.Note; }
            set
            {
                if (Data.Data.Note == value) return;
                Data.Data.Note = value;
                RaisePropertyChanged();
            }
        }

        public bool IsOverdrawn
        {
            get { return Data.Data.IsOverdrawn; }
            set
            {
                if (Data.Data.IsOverdrawn == value) return;
                Data.Data.IsOverdrawn = value;
                RaisePropertyChanged();
            }
        }

        public bool IsExcluded
        {
            get { return Data.Data.IsExcluded; }
            set
            {
                if (Data.Data.IsExcluded == value) return;
                Data.Data.IsExcluded = value;
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