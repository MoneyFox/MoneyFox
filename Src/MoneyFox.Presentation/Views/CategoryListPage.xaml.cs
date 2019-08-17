using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;

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
		}

        protected override void OnAppearing()
        {
            ViewModel.AppearingCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
