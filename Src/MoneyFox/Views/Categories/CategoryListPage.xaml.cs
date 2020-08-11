using MoneyFox.ViewModels.Categories;
using Xamarin.Forms;

namespace MoneyFox.Views.Categories
{
    public partial class CategoryListPage : ContentPage
    {
        private CategoryListViewModel ViewModel => BindingContext as CategoryListViewModel;

        public CategoryListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.CategoryListViewModel;
        }

        protected override async void OnAppearing() => await ViewModel.OnAppearingAsync();
    }
}