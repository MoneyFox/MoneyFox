using MoneyFox.ViewModels.Categories;
using Xamarin.Forms;

namespace MoneyFox.Views.Categories
{
    public partial class CategoryListPage : ContentPage
    {
        private CategoryListViewModel ViewModel => (CategoryListViewModel) BindingContext;

        public CategoryListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.CategoryListViewModel;
        }

        protected override async void OnAppearing() => await ViewModel.InitializeAsync();
    }
}