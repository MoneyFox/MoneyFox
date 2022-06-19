namespace MoneyFox.Win.Pages;

using ViewModels.DataBackup;

public sealed partial class BackupPage
{
    public BackupPage()
    {
        InitializeComponent();
        DataContext = App.GetViewModel<BackupViewModel>();
    }

    public override bool ShowHeader => false;
    internal BackupViewModel ViewModel => (BackupViewModel)DataContext;
}
