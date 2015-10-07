using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class PasswordUserControl
    {
        public PasswordUserControl()
        {
            InitializeComponent();

            DataContext = Mvx.Resolve<PasswordUserControlViewModel>();
        }
    }
}
