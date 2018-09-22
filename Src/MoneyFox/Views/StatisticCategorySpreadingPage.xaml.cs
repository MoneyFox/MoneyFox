using MoneyFox.Business.ViewModels;
using MoneyFox.Dialogs;
using MoneyFox.Foundation.Resources;
using MvvmCross;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
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
	        if (Mvx.IoCProvider.CanResolve<SelectDateRangeDialogViewModel>())
	        {
	            await Navigation.PushPopupAsync(new DateSelectionDialog
	            {
	                BindingContext = Mvx.IoCProvider.Resolve<SelectDateRangeDialogViewModel>()
	            });
	        }
	    }
    }
}