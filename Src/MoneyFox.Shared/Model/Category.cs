using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace MoneyFox.Shared.Model
{
    [Table("Categories")]
    public class Category : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string notes;

        [PrimaryKey, AutoIncrement, Indexed]
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
        ///     Additional details about the category
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