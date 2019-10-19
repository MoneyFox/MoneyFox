using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace MoneyFox.Presentation.Views
{
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
