namespace MoneyFox.Ui.Views.Settings;

public partial class SettingsPage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SettingsViewModel>();
    }

    private SettingsViewModel ViewModel => (SettingsViewModel)BindingContext;

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await ViewModel.InitializeAsync();
    }
}
