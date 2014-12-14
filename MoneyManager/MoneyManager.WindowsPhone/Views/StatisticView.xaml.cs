using System;
using Windows.UI.Xaml;
using MoneyManager.Common;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Dialogs;

namespace MoneyManager.Views
{
    public sealed partial class StatisticView
    {
        private NavigationHelper navigationHelper;

        public StatisticView()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        private async void OpenFilter(object sender, RoutedEventArgs e)
        {
            await new SelectStatisticDialog().ShowAsync();
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


    }
}
