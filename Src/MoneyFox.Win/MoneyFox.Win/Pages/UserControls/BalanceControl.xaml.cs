namespace MoneyFox.Win.Pages.UserControls;

using ViewModels.DesignTime;
using Windows.ApplicationModel;

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