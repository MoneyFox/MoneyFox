using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels;
using MoneyFox.Uwp.ViewModels.Statistic;
using MoneyFox.Uwp.Views.Dialogs;
using System;
using Windows.UI.Xaml;
using static MoneyFox.Uwp.Views.PaymentForCategoryListDialog;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCategorySummaryView
    {
        public override string Header => Strings.CategorySummaryTitle;

        public StatisticCategorySummaryView()
        {
            InitializeComponent();
        }

        private async void SetDate(object sender, RoutedEventArgs e)
        {
            await new SelectDateRangeDialog().ShowAsync();
        }

        private async void CategorySummaryList_ItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            var viewmodel = DataContext as StatisticCategorySummaryViewModel;
            var categorySummaryModel = e.ClickedItem as CategoryOverviewViewModel;

            var parameter = new PaymentForCategoryParameter
            {
                CategoryId = categorySummaryModel.CategoryId,
                TimeRangeFrom = viewmodel.StartDate,
                TimeRangeTo = viewmodel.EndDate
            };

            await new PaymentForCategoryListDialog(parameter).ShowAsync();
        }
    }
}
