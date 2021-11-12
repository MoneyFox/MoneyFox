using MoneyFox.Uwp.ViewModels.Categories;

#nullable enable
namespace MoneyFox.Uwp.Views.Categories
{
    public sealed partial class EditCategoryDialog
    {
        public EditCategoryDialog(int categoryId)
        {
            InitializeComponent();
            DataContext = ViewModelLocator.EditCategoryVm;

            ViewModel.CategoryId = categoryId;
        }

        private EditCategoryViewModel ViewModel => (EditCategoryViewModel)DataContext;
    }
}