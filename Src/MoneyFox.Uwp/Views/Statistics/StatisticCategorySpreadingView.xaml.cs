using System;
using Windows.UI.Xaml;
using MoneyFox.Uwp.Views.Dialogs;
using MoneyFox.Application.Resources;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCategorySpreadingView
    {
        public override string Header => Strings.CategorySpreadingTitle;

        public StatisticCategorySpreadingView()
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
