using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
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