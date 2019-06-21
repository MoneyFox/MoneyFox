using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
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
            ViewModel.AppearingCommand.Execute(null);
        }
    }
}