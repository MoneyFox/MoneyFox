using MoneyFox.Application.Resources;
using MoneyFox.Uwp.Views.Dialogs;
using System;
using Windows.UI.Xaml;

#nullable enable
namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCashFlowView
    {
        public override string Header => Strings.CashFlowStatisticTitle;

        public StatisticCashFlowView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCashFlowVm;
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
