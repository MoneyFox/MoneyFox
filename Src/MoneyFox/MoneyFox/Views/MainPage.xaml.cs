using MoneyFox.Business.ViewModels;
using MvvmCross.Forms.Views.Attributes;
using MvvmCross.Platform;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxTabbedPagePresentation(TabbedPosition.Root, NoHistory = true)]
    public partial class MainPage
	{
		public MainPage ()
		{
			InitializeComponent ();

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