namespace MoneyFox.Ui.Views.Categories.CategorySelection;

public partial class SelectCategoryPage : ContentPage
{
    public SelectCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<CategorySelectionViewModel>();
    }

    private CategorySelectionViewModel SelectionViewModel => (CategorySelectionViewModel)BindingContext;

    protected override void OnAppearing()
    {
        SelectionViewModel.InitializeAsync().GetAwaiter().GetResult();
    }
}
