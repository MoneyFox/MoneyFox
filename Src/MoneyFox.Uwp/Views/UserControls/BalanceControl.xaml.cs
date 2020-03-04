using Windows.ApplicationModel;
using MoneyFox.Uwp.ViewModels.DesignTime;

namespace MoneyFox.Uwp.Views.UserControls
{
    public sealed partial class BalanceControl
    {
        public BalanceControl()
        {
            InitializeComponent();
            if (DesignMode.DesignModeEnabled) DataContext = new DesignTimeBalanceViewViewModel();
        }
    }
}
