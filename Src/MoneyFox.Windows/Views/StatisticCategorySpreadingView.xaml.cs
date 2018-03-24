using System;
using Windows.UI.Xaml;
using MoneyFox.Views;
using MoneyFox.Windows.Views.Dialogs;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Windows.Views
{
    public sealed partial class StatisticCategorySpreadingView
    {
        public StatisticCategorySpreadingView()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
        }

        private void StatisticCategorySpreadingView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new StatisticCategorySpreadingPage { DataContext = ViewModel }.CreateFrameworkElement());
        }
    }
}