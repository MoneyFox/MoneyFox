using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views.UserControls
{
    public sealed partial class SettingsShortcutsUserControl
    {
        public SettingsShortcutsUserControl()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<SettingsShortcutsViewModel>();
        }
    }
}