using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels;
using MoneyFox.Uwp.ViewModels.Statistic;
using MoneyFox.Uwp.Views.Dialogs;
using System;
using System.Linq;
using Windows.UI.Xaml;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCategorySummaryView
    {
        private StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)DataContext;

        public override string Header => Strings.CategorySummaryTitle;

        public StatisticCategorySummaryView()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
        }

        private async void CategorySummaryList_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            var item = e.AddedItems.FirstOrDefault();

            if(item == null) return;
            await ViewModel?.SummaryEntrySelectedCommand.ExecuteAsync(item as CategoryOverviewViewModel);
        }
    }
}
