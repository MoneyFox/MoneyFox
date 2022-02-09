namespace MoneyFox.Win.Pages.Accounts
{
    public sealed partial class AddAccountDialog
    {
        public AddAccountDialog()
        {
            XamlRoot = MainWindow.RootFrame.XamlRoot;
            InitializeComponent();
            DataContext = ViewModelLocator.AddAccountVm;
        }
    }
}