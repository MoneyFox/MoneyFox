#region

using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

#endregion

namespace MoneyManager.Dialogs {
    public sealed partial class CategoryDialog {
        public CategoryDialog(bool isEdit = false) {
            InitializeComponent();

            IsEdit = isEdit;
            if (!isEdit) {
                CategoryData.SelectedCategory = new Category();

                if (!String.IsNullOrEmpty(CategoryListView.SearchText)) {
                    CategoryData.SelectedCategory.Name = CategoryListView.SearchText;
                }
            }
        }

        private CategoryDataAccess CategoryData {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        private CategoryListViewModel CategoryListView {
            get { return ServiceLocator.Current.GetInstance<CategoryListViewModel>(); }
        }

        public bool IsEdit { get; set; }

        private async void DoneOnClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            if (CategoryData.SelectedCategory.Name == String.Empty) {
                var dialog = new MessageDialog(Translation.GetTranslation("NameRequiredMessage"),
                    Translation.GetTranslation("MandatoryField"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));
                dialog.DefaultCommandIndex = 1;

                await dialog.ShowAsync();
            }

            CategoryData.Save(CategoryData.SelectedCategory);

            CategoryListView.SearchText = String.Empty;
            CategoryListView.Search();
        }
    }
}