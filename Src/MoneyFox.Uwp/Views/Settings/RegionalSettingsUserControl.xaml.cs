using MoneyFox.Uwp.ViewModels.DesignTime;
using MoneyFox.Uwp.ViewModels.Settings;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class RegionalSettingsUserControl
    {
        private IRegionalSettingsViewModel ViewModel => DataContext as IRegionalSettingsViewModel;

        public RegionalSettingsUserControl()
        {
            InitializeComponent();

            if(DesignMode.DesignModeEnabled)
                DataContext = new DesignTimeRegionalSettingsViewModel();
        }

        private async void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAvailableCulturesCommand.ExecuteAsync();
        }
    }
}
