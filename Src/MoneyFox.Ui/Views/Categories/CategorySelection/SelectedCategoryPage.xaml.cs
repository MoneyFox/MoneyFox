namespace MoneyFox.Ui.Views.Categories.CategorySelection;

public partial class SelectCategoryPage : ContentPage
{
    public SelectCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<CategorySelectionViewModel>();
    }

    private CategorySelectionViewModel SelectionViewModel => (CategorySelectionViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await SelectionViewModel.InitializeAsync();
    }
}
