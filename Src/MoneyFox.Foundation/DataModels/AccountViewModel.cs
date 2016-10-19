using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoneyFox.Foundation.DataModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string iban;
        private double currentBalance;
        private string note;
        private bool isOverdrawn;

        public int Id
        {
            get { return id; }
            set
            {
                if (id == value) return;
                id = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                RaisePropertyChanged();
            }
        }

        public string Iban
        {
            get { return iban; }
            set
            {
                if (iban == value) return;
                iban = value;
                RaisePropertyChanged();
            }
        }

        public double CurrentBalance
        {
            get { return currentBalance; }
            set
            {
                if (Math.Abs(currentBalance - value) < 0.01) return;
                currentBalance = value;
                RaisePropertyChanged();
            }
        }

        public string Note
        {
            get { return note; }
            set
            {
                if (note == value) return;
                note = value;
                RaisePropertyChanged();
            }
        }

        public bool IsOverdrawn
        {
            get { return isOverdrawn; }
            set
            {
                if (isOverdrawn == value) return;
                isOverdrawn = value;
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