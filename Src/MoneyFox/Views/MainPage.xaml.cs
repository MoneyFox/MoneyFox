using MoneyFox.Business.ViewModels;
using MvvmCross;
using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
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

            // We have to resolve the VM here, since the automagic doesn't yet work the BottomTabbedPage.
            ViewModel = Mvx.Resolve<MainViewModel>();
		}

	    private bool firstTime = true;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (firstTime)
            {
                await ViewModel.ShowInitialViewModelsCommand.ExecuteAsync();
                firstTime = false;
            }
        }
	}
}