using MoneyFox.Foundation.Resources;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BackupPage
    {
		public BackupPage ()
		{
			InitializeComponent();
		    Title = Strings.BackupTitle;
		}
	}
}