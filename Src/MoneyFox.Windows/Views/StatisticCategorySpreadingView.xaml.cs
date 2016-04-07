using System;
using Windows.UI.Xaml;
using MoneyFox.Shared.ViewModels;
using MoneyFox.Windows.Views.Dialogs;

namespace MoneyFox.Windows.Views
{
    public sealed partial class StatisticCategorySpreadingView : IDisposable
    {
        public StatisticCategorySpreadingView()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            SpreadingPlotView.Model = null;
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
            (DataContext as StatisticCategorySpreadingViewModel)?.LoadCommand.Execute(null);
        }
    }
}