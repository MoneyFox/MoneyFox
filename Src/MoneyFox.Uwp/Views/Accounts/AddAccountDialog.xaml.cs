#nullable enable
namespace MoneyFox.Uwp.Views.Accounts
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