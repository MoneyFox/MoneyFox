namespace MoneyFox.Win.Pages.UserControls;

using Windows.ApplicationModel;
using ViewModels.DesignTime;

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
