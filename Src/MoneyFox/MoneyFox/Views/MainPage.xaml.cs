using MvvmCross.Forms.Views.Attributes;
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
		}

	    private bool firstTime = true;

	    protected override async void OnAppearing()
	    {
	        base.OnAppearing();
	        if (firstTime)
	        {
	            await  ViewModel.ShowInitialViewModelsCommand.ExecuteAsync(null);
	            firstTime = false;
	        }
	    }
    }
}