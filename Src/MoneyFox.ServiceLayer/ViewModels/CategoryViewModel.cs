using MoneyFox.DataAccess.Pocos;
using MvvmCross.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    public class CategoryViewModel : MvxViewModel
    {
        public CategoryViewModel(Category category)
        {
            Category = category;
        }

        public Category Category { get; set; }

        public int Id
        {
            get => Category.Data.Id;
            set
            {
                if (Category.Data.Id == value) return;
                Category.Data.Id = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get => Category.Data?.Name;
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
            get => Category.Data?.Note;
            set
            {
                if (Category.Data.Note == value) return;
                Category.Data.Note = value;
                RaisePropertyChanged();
            }
        }
    }
}