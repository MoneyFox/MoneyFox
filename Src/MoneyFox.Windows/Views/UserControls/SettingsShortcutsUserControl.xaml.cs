using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;

namespace MoneyFox.Windows.Views.UserControls
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