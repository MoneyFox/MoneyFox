namespace MoneyFox.Views.Categories
{

    using ViewModels.Categories;
    using Xamarin.Forms;

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
            await ViewModel.InitializeAsync();
        }
    }

}
