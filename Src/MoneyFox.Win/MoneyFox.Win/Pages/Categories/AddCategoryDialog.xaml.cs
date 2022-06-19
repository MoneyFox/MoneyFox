namespace MoneyFox.Win.Pages.Categories;

using ViewModels.Categories;

public sealed partial class AddCategoryDialog
{
    public AddCategoryDialog()
    {
        XamlRoot = MainWindow.RootFrame.XamlRoot;
        InitializeComponent();
        DataContext = App.GetViewModel<AddCategoryViewModel>();
    }
}
