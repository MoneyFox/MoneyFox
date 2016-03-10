using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;

namespace MoneyFox.Windows.Views.UserControls
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