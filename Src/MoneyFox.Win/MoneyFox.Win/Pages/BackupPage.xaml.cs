using MoneyFox.Win.ViewModels.DataBackup;

namespace MoneyFox.Win.Pages
{
    public sealed partial class BackupPage
    {
        public override bool ShowHeader => false;
        public BackupViewModel ViewModel => (BackupViewModel)DataContext;

        public BackupPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.BackupVm;
        }
    }
}