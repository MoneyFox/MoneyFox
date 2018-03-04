using System;
using Windows.UI.Xaml;
using MoneyFox.Business.Views;
using MoneyFox.Windows.Views.Dialogs;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Windows.Views
{
    public sealed partial class StatisticCashFlowView
    {
        public StatisticCashFlowView()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new StatisticCashFlowPage { BindingContext = ViewModel }.CreateFrameworkElement());
        }

        private void StatisticCashFlowView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new StatisticCashFlowPage { BindingContext = ViewModel }.CreateFrameworkElement());
        }
    }
}