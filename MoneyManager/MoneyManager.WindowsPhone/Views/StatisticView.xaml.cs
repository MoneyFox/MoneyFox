using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using MoneyManager.Dialogs;

namespace MoneyManager.Views {
    public sealed partial class StatisticView {
        private readonly NavigationHelper navigationHelper;

        public StatisticView() {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper {
            get { return navigationHelper; }
        }

        private async void OpenFilter(object sender, RoutedEventArgs e) {
            await new SelectStatisticDialog().ShowAsync();
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            ServiceLocator.Current.GetInstance<StatisticViewModel>().SetDefaultCashFlow();
            ServiceLocator.Current.GetInstance<StatisticViewModel>().SetDefaultSpreading();

            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}