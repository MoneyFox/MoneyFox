namespace MoneyFox.Ui.Views.Settings;

using Common.Navigation;

public partial class SettingsPage : IBindablePage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SettingsViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
#if WINDOWS
        var viewModel = (SettingsViewModel)BindingContext;
        viewModel.OnNavigatedAsync(null).GetAwaiter().GetResult();
#endif
    }
}
