using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using MoneyManager.Dialogs;

namespace MoneyManager.Views
{
    public sealed partial class StatisticView
    {
        public StatisticView()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper { get; }

        private async void OpenFilter(object sender, RoutedEventArgs e)
        {
            await new SelectStatisticDialog().ShowAsync();
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ServiceLocator.Current.GetInstance<StatisticViewModel>().SetDefaultCashFlow();
            ServiceLocator.Current.GetInstance<StatisticViewModel>().SetDefaultSpreading();

            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}