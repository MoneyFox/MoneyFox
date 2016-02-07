using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
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