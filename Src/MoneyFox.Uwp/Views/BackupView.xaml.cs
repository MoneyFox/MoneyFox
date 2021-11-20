#nullable enable
using MoneyFox.Uwp.ViewModels.DataBackup;

namespace MoneyFox.Uwp.Views
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