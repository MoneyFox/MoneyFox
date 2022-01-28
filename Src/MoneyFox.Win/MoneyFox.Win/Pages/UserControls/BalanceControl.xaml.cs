using MoneyFox.Win.ViewModels.DesignTime;
using Windows.ApplicationModel;

namespace MoneyFox.Win.Pages.UserControls
{
    public sealed partial class BalanceControl
    {
        public BalanceControl()
        {
            InitializeComponent();
            if(DesignMode.DesignModeEnabled)
            {
                DataContext = new DesignTimeBalanceViewViewModel();
            }
        }
    }
}