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

        private StatisticViewModel StatisticView => Mvx.Resolve<StatisticViewModel>();

        private void LoadStatistic(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            StatisticView.SetCustomCashFlow();
            StatisticView.SetCustomSpreading();
            StatisticView.SetCagtegorySummary();
        }
    }
}