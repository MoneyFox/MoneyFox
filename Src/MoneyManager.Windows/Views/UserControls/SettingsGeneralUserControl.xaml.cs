using Microsoft.Practices.ServiceLocation;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views.UserControls
{
    public sealed partial class SettingsGeneralUserControl
    {
        public SettingsGeneralUserControl()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<SettingsGeneralViewModel>();
        }
    }
}