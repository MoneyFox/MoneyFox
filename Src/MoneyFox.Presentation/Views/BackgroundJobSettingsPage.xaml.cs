using MoneyFox.Foundation.Resources;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BackgroundJobSettingsPage
	{
		public BackgroundJobSettingsPage()
		{
			InitializeComponent ();
		}

	    protected override void OnAppearing()
	    {
	        Title = Strings.BackgroundJobTitle;
            base.OnAppearing();
	    }
	}
}