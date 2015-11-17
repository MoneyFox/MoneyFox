using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Core.ViewModels.Settings;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class TileSettingsUserControl
    {
        public TileSettingsUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<TileSettingsViewModel>();
        }
    }
}