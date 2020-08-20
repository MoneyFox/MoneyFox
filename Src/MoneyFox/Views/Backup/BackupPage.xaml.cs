using MoneyFox.Application.Common;
using MoneyFox.ViewModels.Backup;

namespace MoneyFox.Views.Backup
{
    public partial class BackupPage
    {
        public BackupViewModel ViewModel => (BackupViewModel) BindingContext;

        public BackupPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.BackupViewModel;

            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
