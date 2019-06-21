using MoneyFox.Foundation.Resources;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BackgroundJobSettingsPage
	{
		public BackgroundJobSettingsPage()
		{
			InitializeComponent();
            BindingContext = ViewModelLocator.SettingsBackgroundVm;
        }

        protected override void OnAppearing()
	    {
	        Title = Strings.BackgroundJobTitle;
            base.OnAppearing();
	    }
	}
}