using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;

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
