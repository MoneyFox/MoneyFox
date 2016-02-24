using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
{
    public sealed partial class SettingsGeneralUserControl
    {
        public SettingsGeneralUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<SettingsGeneralViewModel>();
        }
    }
}
