using Windows.ApplicationModel;
using Windows.UI.Xaml;
using MoneyFox.Presentation.ViewModels.DesignTime;
using MoneyFox.Presentation.ViewModels.Settings;

namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class RegionalSettingsUserControl
    {
        private IRegionalSettingsViewModel ViewModel => DataContext as IRegionalSettingsViewModel;

        public RegionalSettingsUserControl()
        {
            InitializeComponent();

            if (DesignMode.DesignModeEnabled) DataContext = new DesignTimeRegionalSettingsViewModel();
        }

        private async void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAvailableCulturesCommand.ExecuteAsync();
        }
    }
}
