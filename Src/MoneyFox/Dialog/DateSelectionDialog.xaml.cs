using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Dialog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DateSelectionDialog : PopupPage
    {
        public DateSelectionDialog ()
		{
			InitializeComponent ();
		}
	}
}