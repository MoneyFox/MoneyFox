using Windows.UI.Xaml.Controls;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views.UserControls
{
    public class MyAccountListActionBarUserControl : ReactiveUserControl<AccountListActionBarViewModel>{
    }

    public sealed partial class AccountListActionBarUserControl
    {
        public AccountListActionBarUserControl() {
            this.InitializeComponent();
        }
    }
}
