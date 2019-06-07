using MoneyFox.Foundation.Resources;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage
	{
		public MainPage ()
		{
		    NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();

		    On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
		    On<Android>().SetBarItemColor(StyleHelper.BarItemColor);
		    On<Android>().SetBarSelectedItemColor(StyleHelper.BarSelectedItemColor);
        }

	    protected override void OnChildAdded(Element child)
        {
	        if ((child as ContentPage)?.Title == "Accounts") {
	            ((ContentPage) child).Title  = Strings.AccountsTitle;
	            ((ContentPage) child).Icon  = StyleHelper.AccountImageSource as FileImageSource;
	        }
	        else if ((child as ContentPage)?.Title == "Statistics")
	        {
	            ((ContentPage)child).Title = Strings.StatisticsTitle;
	            ((ContentPage)child).Icon = StyleHelper.StatisticSelectorImageSource as FileImageSource;
	        }
            else if ((child as ContentPage)?.Title == "Settings") {
	            ((ContentPage) child).Title  = Strings.SettingsTitle;
	            ((ContentPage) child).Icon  = StyleHelper.SettingsImageSource as FileImageSource;
	        }

            base.OnChildAdded(child);
	    }
	}
}