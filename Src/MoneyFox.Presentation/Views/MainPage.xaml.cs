using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxTabbedPagePresentation(TabbedPosition.Root, NoHistory = true)]
    public partial class MainPage
	{
		public MainPage ()
		{
		    NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();

		    On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
		    On<Android>().SetBarItemColor(StyleHelper.BarItemColor);
		    On<Android>().SetBarSelectedItemColor(StyleHelper.BarSelectedItemColor);

            // We have to resolve the VM here, since the auto magic doesn't yet work the BottomTabbedPage.
		    if (Mvx.IoCProvider.CanResolve<MainViewModel>())
		    {
		        ViewModel = Mvx.IoCProvider.Resolve<MainViewModel>();
		    }
		}

	    private bool firstTime = true;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (firstTime)
            {
                await ViewModel.ShowInitialViewModelsCommand.ExecuteAsync().ConfigureAwait(true);
                firstTime = false;
            }
        }

	    protected override void OnChildAdded(Element child)
        {
	        if ((child as MvxContentPage)?.Title == "Accounts") {
	            ((ContentPage) child).Title  = Strings.AccountsTitle;
	            ((ContentPage) child).Icon  = StyleHelper.AccountImageSource as FileImageSource;
	        }
	        else if ((child as MvxContentPage)?.Title == "Statistics")
	        {
	            ((ContentPage)child).Title = Strings.StatisticsTitle;
	            ((ContentPage)child).Icon = StyleHelper.StatisticSelectorImageSource as FileImageSource;
	        }
            else if ((child as MvxContentPage)?.Title == "Settings") {
	            ((ContentPage) child).Title  = Strings.SettingsTitle;
	            ((ContentPage) child).Icon  = StyleHelper.SettingsImageSource as FileImageSource;
	        }

            base.OnChildAdded(child);
	    }
	}
}