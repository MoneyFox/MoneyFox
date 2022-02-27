namespace MoneyFox.Win.Pages.Categories;

using ViewModels.Categories;

public sealed partial class EditCategoryDialog
{
    private EditCategoryViewModel ViewModel => (EditCategoryViewModel)DataContext;

    public EditCategoryDialog(int categoryId)
    {
        XamlRoot = MainWindow.RootFrame.XamlRoot;
        InitializeComponent();
        DataContext = ViewModelLocator.EditCategoryVm;

        ViewModel.CategoryId = categoryId;
    }
}