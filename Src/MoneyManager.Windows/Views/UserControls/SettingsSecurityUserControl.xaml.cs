using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views.UserControls
{
    public sealed partial class SettingsSecurityUserControl
    {
        public SettingsSecurityUserControl()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<SettingsSecurityViewModel>();
        }
    }
}