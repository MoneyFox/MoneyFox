using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Dialogs
{
    public sealed partial class SelectStatisticDialog
    {
        public SelectStatisticDialog()
        {
            InitializeComponent();
        }

        private StatisticViewModel statisticView => Mvx.Resolve<StatisticViewModel>();

        private void LoadStatistic(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            statisticView.SetCustomCashFlow();
            statisticView.SetCustomSpreading();
            statisticView.SetCagtegorySummary();
        }
    }
}