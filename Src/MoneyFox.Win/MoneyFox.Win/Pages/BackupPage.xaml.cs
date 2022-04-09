namespace MoneyFox.Win.Pages;

using ViewModels.DataBackup;

public sealed partial class BackupPage
{
    public BackupPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.BackupVm;
    }

    public override bool ShowHeader => false;
    public BackupViewModel ViewModel => (BackupViewModel)DataContext;
}
