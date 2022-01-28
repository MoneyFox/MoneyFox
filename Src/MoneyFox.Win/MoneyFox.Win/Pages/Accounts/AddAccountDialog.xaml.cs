namespace MoneyFox.Win.Pages.Accounts
{
    public sealed partial class AddAccountDialog
    {
        public AddAccountDialog()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.AddAccountVm;
        }
    }
}