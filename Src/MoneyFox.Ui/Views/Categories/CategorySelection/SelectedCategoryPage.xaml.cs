namespace MoneyFox.Ui.Views.Categories.CategorySelection;

public partial class SelectCategoryPage : ContentPage
{
    public SelectCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SelectCategoryViewModel>();
    }

    private SelectCategoryViewModel ViewModel => (SelectCategoryViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.InitializeAsync().GetAwaiter().GetResult();
    }
}
