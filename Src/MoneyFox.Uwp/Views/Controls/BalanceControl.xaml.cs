using Windows.ApplicationModel;
using MoneyFox.Presentation.ViewModels.DesignTime;

namespace MoneyFox.Uwp.Views.Controls
{
    public sealed partial class BalanceControl
    {
        public BalanceControl()
        {
            InitializeComponent();
            if (DesignMode.DesignModeEnabled)
            {
                DataContext = new DesignTimeBalanceViewViewModel();
            }
        }
    }
}
