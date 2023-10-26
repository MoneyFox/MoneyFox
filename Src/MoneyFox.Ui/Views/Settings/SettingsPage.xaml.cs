namespace MoneyFox.Ui.Views.Settings;

public partial class SettingsPage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SettingsViewModel>();
    }
}
