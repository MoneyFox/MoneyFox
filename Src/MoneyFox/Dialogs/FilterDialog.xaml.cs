using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Dialogs
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FilterDialog : PopupPage
    {
        public FilterDialog ()
		{
			InitializeComponent ();
		}
	}
}