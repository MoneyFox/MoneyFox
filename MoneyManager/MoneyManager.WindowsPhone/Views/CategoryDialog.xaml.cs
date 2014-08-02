using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.ViewModels.Data;
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

        public CategoryViewModel CategorievViewModel
        {
            get { return new ViewModelLocator().CategoryViewModel; }
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
                CategorievViewModel.Save(category);
            }
            else
            {
                categoryToUpdate.Name = TextboxCategoryName.Text;
                CategorievViewModel.Update(categoryToUpdate);
            }
        }
    }
}