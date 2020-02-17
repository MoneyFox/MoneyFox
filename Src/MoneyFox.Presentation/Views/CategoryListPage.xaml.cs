using System;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Ui.Shared.Utilities;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class CategoryListPage
    {
        private CategoryListViewModel ViewModel => BindingContext as CategoryListViewModel;

        public CategoryListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.CategoryListVm;

            Title = Strings.CategoriesTitle;


            if (Device.RuntimePlatform == Device.iOS)
            {
                var addItem = new ToolbarItem
                {
                    Text = Strings.AddTitle,
                    Priority = 0,
                    Order = ToolbarItemOrder.Primary
                };
                addItem.Clicked += AddCategoryClick;
                ToolbarItems.Add(addItem);
            }
        }

        protected override void OnAppearing()
        {
            ViewModel.AppearingCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private void AddCategoryClick(object sender, EventArgs e)
        {
            ViewModel.CreateNewCategoryCommand.Execute(null);
        }
    }
}
