using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
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