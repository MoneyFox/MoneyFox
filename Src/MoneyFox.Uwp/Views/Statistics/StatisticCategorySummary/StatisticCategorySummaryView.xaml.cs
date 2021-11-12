﻿using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

#nullable enable
namespace MoneyFox.Uwp.Views.Statistics.StatisticCategorySummary
{
    public sealed partial class StatisticCategorySummaryView
    {
        public StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)DataContext;

        public override string Header => Strings.CategorySummaryTitle;

        public StatisticCategorySummaryView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCategorySummaryVm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.LoadedCommand.Execute(null);

        private void OpenFilterFlyout(object sender, RoutedEventArgs e) => FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);

        private void CategorySummaryList_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            object item = e.AddedItems.FirstOrDefault();

            if(!(item is CategoryOverviewViewModel vm))
            {
                return;
            }

            ViewModel.SummaryEntrySelectedCommand.Execute(vm);
        }
    }
}
