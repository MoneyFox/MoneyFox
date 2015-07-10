using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

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
                CategoryRepository.Selected = new Category();

                if (!string.IsNullOrEmpty(CategoryListView.SearchText))
                {
                    CategoryRepository.Selected.Name = CategoryListView.SearchText;
                }
            }
        }

        private IRepository<Category> CategoryRepository
        {
            get { return ServiceLocator.Current.GetInstance<IRepository<Category>>(); }
        }

        private CategoryListViewModel CategoryListView
        {
            get { return ServiceLocator.Current.GetInstance<CategoryListViewModel>(); }
        }

        public bool IsEdit { get; set; }

        private async void DoneOnClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (CategoryRepository.Selected.Name == string.Empty)
            {
                var dialog = new MessageDialog(Translation.GetTranslation("NameRequiredMessage"),
                    Translation.GetTranslation("MandatoryField"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));
                dialog.DefaultCommandIndex = 1;

                await dialog.ShowAsync();
            }

            CategoryRepository.Save(CategoryRepository.Selected);

            CategoryListView.SearchText = string.Empty;
            CategoryListView.Search();
        }
    }
}