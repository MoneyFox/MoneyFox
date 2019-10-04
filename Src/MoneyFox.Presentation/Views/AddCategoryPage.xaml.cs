using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.Views
{
	public partial class AddCategoryPage
    {
        private AddCategoryViewModel ViewModel => BindingContext as AddCategoryViewModel;

        public AddCategoryPage ()
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AddCategoryVm;

            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
