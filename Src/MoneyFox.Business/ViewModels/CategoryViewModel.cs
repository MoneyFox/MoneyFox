using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoneyFox.Foundation.DataModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string notes;

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


        /// <summary>
        ///     Additional details about the CategoryViewModel
        /// </summary>
        public string Notes
        {
            get { return notes; }
            set
            {
                if (notes == value) return;
                notes = value;
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