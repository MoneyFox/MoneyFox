using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
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
        private IRepository<Category> CategoryRepository => Mvx.Resolve<IRepository<Category>>();

        private CategoryListViewModel CategoryListView => Mvx.Resolve<CategoryListViewModel>();

        public bool IsEdit { get; set; }

        private async void DoneOnClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (CategoryRepository.Selected.Name == string.Empty)
            {
                var dialogService = Mvx.Resolve<IDialogService>();

                await dialogService.ShowMessage(Strings.MandatoryFieldEmptryTitle, Strings.NameRequiredMessage);
            }

            CategoryRepository.Save(CategoryRepository.Selected);

            CategoryListView.SearchText = string.Empty;
            CategoryListView.Search();
        }
    }
}