using Windows.ApplicationModel;
using MoneyFox.Windows.DesignTime;

namespace MoneyFox.Windows.Views.UserControls
{
    public sealed partial class BalanceUserControl
    {
        public BalanceUserControl()
        {
            InitializeComponent();
            if (DesignMode.DesignModeEnabled)
            {
                DataContext = new DesignTimeBalanceViewModel();
            }
        }
    }
}