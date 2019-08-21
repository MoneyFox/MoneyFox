using System;
using Windows.UI.Xaml;
using MoneyFox.Presentation;
using MoneyFox.Presentation.Views;
using MoneyFox.Uwp.Views.Dialogs;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class StatisticCategorySummaryView
    {
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

        private void StatisticCategorySummaryView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new StatisticCategorySummaryPage { BindingContext = DataContext }.CreateFrameworkElement());
        }
    }
}
