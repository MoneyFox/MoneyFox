using MoneyFox.Foundation.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatisticCategorySpreadingPage
	{
		public StatisticCategorySpreadingPage ()
		{
			InitializeComponent ();
		    Title = Strings.CategorySpreadingTitle;

		    var filterItem = new ToolbarItem
		    {
		        Command = new Command(OpenDialog),
		        Text = Strings.SelectDateLabel,
		        Priority = 0,
		        Order = ToolbarItemOrder.Primary
		    };

		    ToolbarItems.Add(filterItem);
        }

	    private async void OpenDialog()
	    {
            // TODO Reactivate.
	        //if (Mvx.IoCProvider.CanResolve<SelectDateRangeDialogViewModel>())
	        //{
	        //    await Navigation.PushPopupAsync(new DateSelectionDialog
	        //    {
	        //        BindingContext = Mvx.IoCProvider.Resolve<SelectDateRangeDialogViewModel>()
	        //    });
	        //}
	    }
    }
}