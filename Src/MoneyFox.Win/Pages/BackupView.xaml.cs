using MoneyFox.Win.ViewModels.DataBackup;

namespace MoneyFox.Win.Pages
{
    public sealed partial class BackupView
    {
        public override bool ShowHeader => false;
        public BackupViewModel ViewModel => (BackupViewModel)DataContext;

        public BackupView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.BackupVm;
        }
    }
}