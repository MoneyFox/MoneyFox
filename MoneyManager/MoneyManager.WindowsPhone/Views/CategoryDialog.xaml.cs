using MoneyManager.Models;
using MoneyTracker.Src;
using Windows.UI.Xaml.Controls;

namespace MoneyManager.Views
{
    public sealed partial class CategoryDialog
    {
        private readonly Category categoryToUpdate;

        public CategoryDialog()
        {
            InitializeComponent();
        }

        public CategoryDialog(Category category)
        {
            InitializeComponent();
            categoryToUpdate = category;
            TextboxCategoryName.Text = category.Name;
            PrimaryButtonText = Utilities.GetTranslation("Update");
        }

        private void DoneOnClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (categoryToUpdate == null)
            {
                var category = new Category { Name = TextboxCategoryName.Text };
                App.CategoryViewModel.Save(category);
            }
            else
            {
                categoryToUpdate.Name = TextboxCategoryName.Text;
                App.CategoryViewModel.Update(categoryToUpdate);
            }
        }
    }
}