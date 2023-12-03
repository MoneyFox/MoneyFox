namespace MoneyFox.Ui.Views.Settings;

using Common.Navigation;

public partial class SettingsPage : IBindablePage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SettingsViewModel>();
    }

    private SettingsViewModel ViewModel => (SettingsViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.InitializeAsync().GetAwaiter().GetResult();
    }
}
