namespace MoneyFox.Ui.Views.Backup;

using Common.Navigation;

public partial class BackupPage : IBindablePage
{
    public BackupPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<BackupViewModel>();
    }

    internal BackupViewModel ViewModel => (BackupViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.InitializeCommand.Execute(null);
    }
}
