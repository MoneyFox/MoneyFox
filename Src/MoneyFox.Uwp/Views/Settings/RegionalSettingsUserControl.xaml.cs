using MoneyFox.Presentation.ViewModels.DesignTime;
using Windows.ApplicationModel;

namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class RegionalSettingsUserControl
    {
        public RegionalSettingsUserControl()
        {
            InitializeComponent();

            if (DesignMode.DesignModeEnabled) DataContext = new DesignTimeRegionalSettingsViewModel();
        }
    }
}
