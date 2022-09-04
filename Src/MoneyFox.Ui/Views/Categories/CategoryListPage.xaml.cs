namespace MoneyFox.Ui.Views.Categories;

using ViewModels.Categories;

public partial class CategoryListPage : ContentPage
{
    public CategoryListPage()
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
