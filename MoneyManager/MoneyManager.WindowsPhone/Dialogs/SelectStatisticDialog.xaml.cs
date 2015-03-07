using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Views;

namespace MoneyManager.Dialogs {
    public sealed partial class SelectStatisticDialog {
        public SelectStatisticDialog() {
            InitializeComponent();
        }

        private StatisticViewModel statisticView {
            get { return ServiceLocator.Current.GetInstance<StatisticViewModel>(); }
        }

        private void LoadStatistic(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            if (LicenseHelper.IsFeaturepackLicensed) {
                statisticView.SetCustomCashFlow();
                statisticView.SetCustomSpreading();
            } else {
                var dialog = new MessageDialog(Translation.GetTranslation("ShowFeatureNotLicensedMessage"),
                    Translation.GetTranslation("FeatureNotLicensedTitle"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("RedirectLabel"), GoToPurchase));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("BackLabel")));
                dialog.ShowAsync();
            }
        }

        private void GoToPurchase(IUICommand command) {
            ((Frame) Window.Current.Content).Navigate(typeof (LicenseView));
        }
    }
}