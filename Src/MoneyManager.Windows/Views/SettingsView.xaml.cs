using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<SettingDefaultsViewModel>();

            Mvx.Resolve<CategoryListViewModel>().IsSettingCall = true;
        }
    }
}