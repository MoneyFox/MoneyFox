namespace MoneyFox.Ui.Views.Backup;

using Common.Navigation;

public partial class BackupPage : IBindablePage
{
    public BackupPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<BackupViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
#if WINDOWS
        var viewModel = (BackupViewModel)BindingContext;
        await viewModel.OnNavigatedAsync(null);
#endif
    }
}
