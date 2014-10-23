using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;

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

        private async void DoneOnClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (CategoryData.SelectedCategory.Name == String.Empty)
            {
                var dialog = new MessageDialog(Translation.GetTranslation("NameRequiredMessage"),
                    Translation.GetTranslation("NameRequiredTitle"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));
                dialog.DefaultCommandIndex = 1;

                await dialog.ShowAsync();
            }

            if (IsEdit)
            {
                CategoryData.Update(CategoryData.SelectedCategory);
            }
            else
            {
                CategoryData.Save(CategoryData.SelectedCategory);
            }

            ServiceLocator.Current.GetInstance<SelectCategoryViewModel>().SearchText = String.Empty;
        }
    }
}