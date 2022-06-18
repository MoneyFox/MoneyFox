namespace MoneyFox.Win.Pages.Settings;

using ViewModels.Settings;

public sealed partial class SettingsPage : BasePage
{
    public SettingsPage()
    {
        InitializeComponent();
        DataContext = App.GetViewModel<WindowsSettingsViewModel>();
    }

    private WindowsSettingsViewModel ViewModel => (WindowsSettingsViewModel)DataContext;
}
