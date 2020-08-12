using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.ViewModels.Backup;

namespace MoneyFox.Views
{
    public partial class BackupPage
    {
        public BackupViewModel ViewModel => BindingContext as BackupViewModel;

        public BackupPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.BackupViewModel;

            Title = Strings.BackupTitle;

            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
