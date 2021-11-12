using MoneyFox.ViewModels.Categories;
using Xamarin.Forms;

namespace MoneyFox.Views.Categories
{
    public partial class CategoryListPage : ContentPage
    {
        public CategoryListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.CategoryListViewModel;
        }

        private CategoryListViewModel ViewModel => (CategoryListViewModel)BindingContext;

        protected override async void OnAppearing()
        {
            ViewModel.Subscribe();
            await ViewModel.InitializeAsync();
        }

        protected override void OnDisappearing()
            => ViewModel.Unsubscribe();
    }
}