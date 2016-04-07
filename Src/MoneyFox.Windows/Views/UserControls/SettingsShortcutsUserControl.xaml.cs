using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views.UserControls
{
    public sealed partial class SettingsShortcutsUserControl
    {
        public SettingsShortcutsUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<SettingsShortcutsViewModel>();
        }
    }
}