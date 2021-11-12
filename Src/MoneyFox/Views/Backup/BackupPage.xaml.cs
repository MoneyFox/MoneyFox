using MoneyFox.ViewModels.DataBackup;

namespace MoneyFox.Views.Backup
{
    public partial class BackupPage
    {
        public BackupPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.BackupViewModel;
        }

        public BackupViewModel ViewModel => (BackupViewModel)BindingContext;

        protected override void OnAppearing() => ViewModel.InitializeCommand.Execute(null);
    }
}