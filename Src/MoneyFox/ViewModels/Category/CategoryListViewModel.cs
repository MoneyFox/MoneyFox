using GalaSoft.MvvmLight.Command;
using MoneyFox.Extensions;
using MoneyFox.Groups;
using MoneyFox.Views.Categories;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Category
{
    public class CategoryListViewModel : BaseViewModel
    {
        public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> Categories { get; set; } = new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>
        {
            new AlphaGroupListGroupCollection<CategoryViewModel>("F")
            {
                new CategoryViewModel{ Name = "Food" }
            },
            new AlphaGroupListGroupCollection<CategoryViewModel>("Included")
            {
                new CategoryViewModel{ Name = "Drinks"},
                new CategoryViewModel{ Name = "Drugs"}
            }
        };

        public RelayCommand GoToAddCategoryCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddCategoryRoute));

        public RelayCommand<CategoryViewModel> GoToEditCategoryCommand
            => new RelayCommand<CategoryViewModel>(async (categoryViewModel)
                => await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditCategoryPage(categoryViewModel.Id)) { BarBackgroundColor = Color.Transparent }));
    }
}
