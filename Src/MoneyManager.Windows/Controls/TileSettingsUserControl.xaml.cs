using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

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