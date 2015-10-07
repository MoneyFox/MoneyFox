using System;
using Windows.UI.Xaml;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class StatisticsView
    {
        public StatisticsView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<StatisticViewModel>();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectStatisticDialog().ShowAsync();
        }
    }
}