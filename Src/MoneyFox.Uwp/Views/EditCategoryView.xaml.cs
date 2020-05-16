using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditCategoryView : ContentDialog
    {
        private EditCategoryViewModel ViewModel => DataContext as EditCategoryViewModel;

        public EditCategoryView(int categoryId)
        {
            InitializeComponent();

            ViewModel.CategoryId = categoryId;
        }
    }
}
