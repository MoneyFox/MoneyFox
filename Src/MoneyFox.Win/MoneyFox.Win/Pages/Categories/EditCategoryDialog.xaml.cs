using MoneyFox.Win.ViewModels.Categories;

namespace MoneyFox.Win.Pages.Categories
{
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
}