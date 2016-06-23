using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views.UserControls {
    public sealed partial class SettingsPersonalizationUserControl {
        public SettingsPersonalizationUserControl() {
            InitializeComponent();
            DataContext = Mvx.Resolve<PersonalizationUserControlViewModel>();
        }
    }
}