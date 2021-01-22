using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp.Views.Categories
{
    public sealed partial class EditCategoryDialog : ContentDialog
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
