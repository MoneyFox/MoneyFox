using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using Windows.UI.Xaml.Controls;

namespace MoneyManager.Dialogs
{
    public sealed partial class CategoryDialog
    {
        public CategoryDialog(bool isEdit = false)
        {
            InitializeComponent();

            IsEdit = isEdit;
            if (!isEdit)
            {
                CategoryData.SelectedCategory = new Category();
            }
        }

        private CategoryDataAccess CategoryData
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        public bool IsEdit { get; set; }

        private void DoneOnClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (IsEdit)
            {
                CategoryData.Update(CategoryData.SelectedCategory);
            }
            else
            {
                CategoryData.Save(CategoryData.SelectedCategory);
            }
        }
    }
}