using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class SettingsRegionalPage : ContentPage
    {
        public SettingsRegionalPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.RegionalSettingsVm;
        }
    }
}
