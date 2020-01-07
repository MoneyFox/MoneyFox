using MoneyFox.Presentation.ViewModels.DesignTime;
using MoneyFox.Presentation.ViewModels.Settings;
using Windows.ApplicationModel;

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

        private async void ComboBox_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.LoadAvailableCulturesCommand.ExecuteAsync();
        }
    }
}
