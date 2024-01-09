namespace MoneyFox.Ui.Views.Categories;

using Common.Navigation;

public partial class CategoryListPage : IBindablePage
{
    public CategoryListPage()
    {
        InitializeComponent();
    }

    public CategoryListViewModel ViewModel => (CategoryListViewModel)BindingContext;
}
