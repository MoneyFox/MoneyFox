namespace MoneyFox.Ui.Views.Categories;

public partial class DesktopCategoryListPage : ContentPage
{
    public DesktopCategoryListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<CategoryListViewModel>();
    }

    private CategoryListViewModel ViewModel => (CategoryListViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync();
    }
}
