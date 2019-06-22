using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell
    {
		public AppShell()
		{
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            BindingContext = ViewModelLocator.ShellVm;

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}