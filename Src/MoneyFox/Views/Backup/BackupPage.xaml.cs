namespace MoneyFox.Views.Backup
{

    using ViewModels.DataBackup;

    public partial class BackupPage
    {
        public BackupPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.BackupViewModel;
        }

        public BackupViewModel ViewModel => (BackupViewModel)BindingContext;

        protected override void OnAppearing()
        {
            ViewModel.InitializeCommand.Execute(null);
        }
    }

}
