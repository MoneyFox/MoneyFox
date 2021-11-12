using MoneyFox.Uwp.ViewModels.Categories;

#nullable enable
namespace MoneyFox.Uwp.Views.Categories
{
    public sealed partial class EditCategoryDialog
    {
        private EditCategoryViewModel ViewModel => (EditCategoryViewModel)DataContext;

        public EditCategoryDialog(int categoryId)
        {
            InitializeComponent();
            DataContext = ViewModelLocator.EditCategoryVm;

            ViewModel.CategoryId = categoryId;
        }
    }
}
