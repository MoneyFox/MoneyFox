using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views.UserControls {
    public sealed partial class SettingsSecurityUserControl {
        public SettingsSecurityUserControl() {
            InitializeComponent();
            DataContext = Mvx.Resolve<SettingsSecurityViewModel>();
        }
    }
}