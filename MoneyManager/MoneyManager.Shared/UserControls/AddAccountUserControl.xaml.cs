using MoneyManager;

namespace MoneyTracker.UserControls
{
    public sealed partial class AddAccountUserControl
    {
        public AddAccountUserControl()
        {
            InitializeComponent();

            DataContext = App.AccountViewModel.SelectedAccount;
        }
    }
}