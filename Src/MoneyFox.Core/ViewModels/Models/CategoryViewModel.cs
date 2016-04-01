using MoneyFox.Core.DatabaseModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoneyFox.Core.ViewModels.Models
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private Category category;

        public CategoryViewModel() 
            : this(new Category()) { }

        public CategoryViewModel(Category category)
        {
            this.category = category;
        }

        public int Id => category.Id;

        public string Name
        {
            get { return category.Name; }
            set { category.Name = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
