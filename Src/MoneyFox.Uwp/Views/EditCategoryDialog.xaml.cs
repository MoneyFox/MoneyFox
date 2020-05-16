using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditCategoryDialog : ContentDialog
    {
        private EditCategoryViewModel ViewModel => DataContext as EditCategoryViewModel;

        public EditCategoryDialog(int categoryId)
        {
            InitializeComponent();

            ViewModel.CategoryId = categoryId;
        }
    }
}
