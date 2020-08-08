using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Categories
{
    public abstract class ModifyCategoryViewModel : BaseViewModel
    {
        public RelayCommand SaveCommand => new RelayCommand(async () => await SaveCategoryBaseAsync());


        private CategoryViewModel selectedCategory = new CategoryViewModel();

        /// <summary>
        /// The currently selected CategoryViewModel
        /// </summary>
        public CategoryViewModel SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                RaisePropertyChanged();
            }
        }

        protected virtual async Task SaveCategoryBaseAsync()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
