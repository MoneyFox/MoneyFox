using MoneyFox.Application.Resources;
using MoneyFox.Uwp.Views.Dialogs;
using System;
using Windows.UI.Xaml;

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
            await new SelectDateRangeDialog
                  {
                      DataContext = ViewModelLocator.SelectDateRangeDialogVm
                  }.ShowAsync();
        }
    }
}
