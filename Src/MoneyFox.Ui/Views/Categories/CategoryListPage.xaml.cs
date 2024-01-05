namespace MoneyFox.Ui.Views.Categories;

using Common.Navigation;

public partial class CategoryListPage : IBindablePage
{
    public CategoryListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<CategoryListViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
#if WINDOWS
        var viewModel = (CategoryListViewModel)BindingContext;
        viewModel.OnNavigatedAsync(null).GetAwaiter().GetResult();
#endif
    }
}
