namespace MoneyFox.Ui.Views.Categories;

using Core.Resources;
using ViewModels.Categories;

public partial class AddCategoryPage
{
    public AddCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddCategoryViewModel>();
    }
}
