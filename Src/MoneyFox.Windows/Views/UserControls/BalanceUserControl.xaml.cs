using Windows.ApplicationModel;
using MoneyFox.Business.ViewModels.DesignTime;

namespace MoneyFox.Windows.Views.UserControls
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