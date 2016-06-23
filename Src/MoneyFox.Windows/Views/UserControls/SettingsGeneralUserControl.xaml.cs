using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views.UserControls {
    public sealed partial class SettingsGeneralUserControl {
        public SettingsGeneralUserControl() {
            InitializeComponent();
            DataContext = Mvx.Resolve<SettingsGeneralViewModel>();
        }
    }
}