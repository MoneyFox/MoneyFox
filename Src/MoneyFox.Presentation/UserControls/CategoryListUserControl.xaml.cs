using System;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace MoneyFox.Presentation.UserControls
{
    public partial class CategoryListUserControl
    {
        private AbstractCategoryListViewModel ViewModel => (AbstractCategoryListViewModel) BindingContext;

        public CategoryListUserControl()
        {
            InitializeComponent();

            CategoryList.ItemTapped += (sender, args) =>
            {
                CategoryList.SelectedItem = null;
                ViewModel.ItemClickCommand.Execute(args.Item);
            };

            CategoryList.On<Android>().SetIsFastScrollEnabled(true);
        }

        private void EditCategory(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            if (menuItem == null) return;

            ViewModel.EditCategoryCommand.Execute(menuItem.CommandParameter);
        }

        private void DeleteCategory(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            if (menuItem == null) return;

            ViewModel.DeleteCategoryCommand.ExecuteAsync((CategoryViewModel) menuItem.CommandParameter).FireAndForgetSafeAsync();
        }

        private void AddCategoryClick(object sender, EventArgs e)
        {
            ViewModel.CreateNewCategoryCommand.Execute(null);
        }

        private void SearchTermChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.SearchCommand.ExecuteAsync(e.NewTextValue).FireAndForgetSafeAsync();
        }
    }
}
