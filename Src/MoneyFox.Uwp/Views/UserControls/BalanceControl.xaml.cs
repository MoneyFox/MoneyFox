using MoneyFox.Uwp.ViewModels.DesignTime;
using Windows.ApplicationModel;

#nullable enable
namespace MoneyFox.Uwp.Views.UserControls
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