namespace MoneyFox.Ui.Views.Categories;

public partial class CategoryListPage : ContentPage
{
    public CategoryListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<CategoryListViewModel>();
    }

    private CategoryListViewModel ViewModel => (CategoryListViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
        ViewModel.InitializeAsync().GetAwaiter().GetResult();
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }
}
