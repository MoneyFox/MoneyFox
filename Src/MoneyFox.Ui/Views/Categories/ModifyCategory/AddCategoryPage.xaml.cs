namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

public partial class AddCategoryPage
{
    public AddCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddCategoryViewModel>();
    }
}
