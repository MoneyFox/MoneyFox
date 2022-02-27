namespace MoneyFox.Win.Pages.Settings;

using ViewModels.Settings;

public sealed partial class SettingsPage : BasePage
{
    private WindowsSettingsViewModel ViewModel => (WindowsSettingsViewModel)DataContext;

    public SettingsPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.SettingsVm;
    }
}