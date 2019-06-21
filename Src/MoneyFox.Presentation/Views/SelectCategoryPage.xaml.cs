using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectCategoryPage 
	{
        private SelectCategoryListViewModel ViewModel => BindingContext as SelectCategoryListViewModel;

		public SelectCategoryPage ()
		{
			InitializeComponent ();
		    Title = Strings.SelectCategoryTitle;
        }

        protected override void OnAppearing()
        {
            ViewModel.AppearingCommand.Execute(null);
        }
    }
}