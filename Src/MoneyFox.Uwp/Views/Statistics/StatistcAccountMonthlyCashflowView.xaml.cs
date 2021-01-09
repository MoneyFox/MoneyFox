using MoneyFox.Application.Resources;
using MoneyFox.Uwp.Views.Dialogs;
using MoneyFox.ViewModels.Statistics;
using System;
using Windows.UI.Xaml;

#nullable enable
namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatistcAccountMonthlyCashflowView
    {
        public StatisticAccountMonthlyCashflowViewModel ViewModel => (StatisticAccountMonthlyCashflowViewModel)DataContext;

        public override string Header => Strings.MonthlyCashflowTitle;

        public StatistcAccountMonthlyCashflowView()
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
