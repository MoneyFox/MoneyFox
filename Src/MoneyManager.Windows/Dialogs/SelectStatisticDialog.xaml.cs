using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;

namespace MoneyManager.Windows.Dialogs
{
    public sealed partial class SelectStatisticDialog
    {
        public SelectStatisticDialog()
        {
            InitializeComponent();
        }

        private StatisticViewModel statisticView => ServiceLocator.Current.GetInstance<StatisticViewModel>();

        private void LoadStatistic(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            statisticView.SetCustomCashFlow();
            statisticView.SetCustomSpreading();
            statisticView.SetCagtegorySummary();
        }
    }
}