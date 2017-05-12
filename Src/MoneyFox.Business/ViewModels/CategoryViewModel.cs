using System.ComponentModel;
using System.Runtime.CompilerServices;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Business.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        public CategoryViewModel(Category category)
        {
            Category = category;
        }

        public Category Category { get; set; }

        public int Id
        {
            get { return Category.Data.Id; }
            set
            {
                if (Category.Data.Id == value) return;
                Category.Data.Id = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get { return Category.Data?.Name; }
            set
            {
                if (Category.Data.Name == value) return;
                Category.Data.Name = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     Additional details about the CategoryViewModel
        /// </summary>
        public string Notes
        {
            get { return Category.Data?.Note; }
            set
            {
                if (Category.Data.Note == value) return;
                Category.Data.Note = value;
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