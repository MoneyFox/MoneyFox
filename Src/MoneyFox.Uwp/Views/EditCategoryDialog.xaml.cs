using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditCategoryDialog : ContentDialog
    {
        private EditCategoryViewModel ViewModel => (EditCategoryViewModel)DataContext;

        public EditCategoryDialog(int categoryId)
        {
            InitializeComponent();

            ViewModel.CategoryId = categoryId;
        }
    }
}
