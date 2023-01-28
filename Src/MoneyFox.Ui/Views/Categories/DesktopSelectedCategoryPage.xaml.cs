namespace MoneyFox.Ui.Views.Categories;

public partial class DesktopSelectedCategoryPage : ContentPage
{
    public DesktopSelectedCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SelectCategoryViewModel>();
    }

    private SelectCategoryViewModel ViewModel => (SelectCategoryViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync();
    }
}
