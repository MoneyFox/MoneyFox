using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;

namespace MoneyManager.Dialogs
{
    public sealed partial class CategoryDialog
    {
        public CategoryDialog()
        {
            InitializeComponent();

            CategoryData.SelectedCategory = new Category();
        }

        private CategoryDataAccess CategoryData
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        private void DoneOnClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            CategoryData.Save(CategoryData.SelectedCategory);
        }
    }
}