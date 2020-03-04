using MoneyFox.Uwp.ViewModels.DesignTime;
using Windows.ApplicationModel;

namespace MoneyFox.Uwp.Views.UserControls
{
    public sealed partial class BalanceControl
    {
        public BalanceControl()
        {
            InitializeComponent();
            if(DesignMode.DesignModeEnabled)
                DataContext = new DesignTimeBalanceViewViewModel();
        }
    }
}
