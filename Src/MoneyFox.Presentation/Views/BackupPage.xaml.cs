using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Ui.Shared.Utilities;

namespace MoneyFox.Presentation.Views
{
    public partial class BackupPage
    {
        public BackupViewModel ViewModel => BindingContext as BackupViewModel;

        public BackupPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.BackupVm;

            Title = Strings.BackupTitle;

            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
