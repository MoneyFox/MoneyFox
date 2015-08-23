using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Windows.Dialogs
{
    public sealed partial class CategoryDialog
    {
        public CategoryDialog(bool isEdit = false)
        {
            InitializeComponent();

            //TODO Refactor
            IsEdit = isEdit;
            if (!isEdit)
            {
                CategoryRepository.Selected = new Category();

                if (!string.IsNullOrEmpty(CategoryListView.SearchText))
                {
                    CategoryRepository.Selected.Name = CategoryListView.SearchText;
                }
            }
        }

        //TODO: refactor
        private IRepository<Category> CategoryRepository => ServiceLocator.Current.GetInstance<IRepository<Category>>();

        private CategoryListViewModel CategoryListView => ServiceLocator.Current.GetInstance<CategoryListViewModel>();

        public bool IsEdit { get; set; }

        private async void DoneOnClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (CategoryRepository.Selected.Name == string.Empty)
            {
                var dialogService = ServiceLocator.Current.GetInstance<IDialogService>();

                await dialogService.ShowMessage(Strings.MandatoryFieldEmptryTitle, Strings.NameRequiredMessage);
            }

            CategoryRepository.Save(CategoryRepository.Selected);

            CategoryListView.SearchText = string.Empty;
            CategoryListView.Search();
        }
    }
}