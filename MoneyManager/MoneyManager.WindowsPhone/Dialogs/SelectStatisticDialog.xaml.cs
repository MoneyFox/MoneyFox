using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;

namespace MoneyManager.Dialogs
{
    public sealed partial class SelectStatisticDialog
    {
        public SelectStatisticDialog()
        {
            InitializeComponent();
        }

        private StatisticViewModel statisticView
        {
            get { return ServiceLocator.Current.GetInstance<StatisticViewModel>(); }
        }

        private void LoadStatistic(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            statisticView.SetCustomCashFlow();
            statisticView.SetCustomSpreading();
        }
    }
}
