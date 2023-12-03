namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

using Common.Navigation;

public partial class AddCategoryPage: IBindablePage
{
    public AddCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddCategoryViewModel>();
    }

    private AddCategoryViewModel ViewModel => (AddCategoryViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }
}
