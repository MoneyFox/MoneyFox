using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.Views
{
	public partial class SelectCategoryPage 
	{
        private SelectCategoryListViewModel ViewModel => BindingContext as SelectCategoryListViewModel;

		public SelectCategoryPage ()
		{
			InitializeComponent();
            BindingContext = ViewModelLocator.SelectCategoryListVm;

            Title = Strings.SelectCategoryTitle;
        }

        protected override void OnAppearing()
        {
            ViewModel.AppearingCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
