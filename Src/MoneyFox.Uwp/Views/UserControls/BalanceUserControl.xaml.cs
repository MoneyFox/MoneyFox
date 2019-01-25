using Windows.ApplicationModel;
using MoneyFox.ServiceLayer.ViewModels.DesignTime;

namespace MoneyFox.Uwp.Views.UserControls
{
    public sealed partial class BalanceUserControl
    {
        public BalanceUserControl()
        {
            InitializeComponent();
            if (DesignMode.DesignModeEnabled)
            {
                DataContext = new DesignTimeBalanceViewViewModel();
            }
        }
    }
}