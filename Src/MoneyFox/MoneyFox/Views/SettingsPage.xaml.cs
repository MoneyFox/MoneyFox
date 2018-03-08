using MvvmCross.Forms.Views.Attributes;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxTabbedPagePresentation(WrapInNavigationPage = false, Title = "Settings", Icon = "ic_settings")]
    public partial class SettingsPage 
	{
		public SettingsPage ()
		{
			InitializeComponent ();
		}
	}
}