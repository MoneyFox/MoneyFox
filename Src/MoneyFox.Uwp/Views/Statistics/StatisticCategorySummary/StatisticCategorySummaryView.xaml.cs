using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;
using MoneyFox.Uwp.Views.Dialogs;
using System;
using System.Linq;
using Windows.UI.Xaml;

namespace MoneyFox.Uwp.Views.Statistics.StatisticCategorySummary
{
    public sealed partial class StatisticCategorySummaryView
    {
        public StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)DataContext;

        public override string Header => Strings.CategorySummaryTitle;

        public StatisticCategorySummaryView()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
        }

        private void CategorySummaryList_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            var item = e.AddedItems.FirstOrDefault();

            if(item == null) return;
            ViewModel.SummaryEntrySelectedCommand.Execute((CategoryOverviewViewModel)item);
        }
    }
}
