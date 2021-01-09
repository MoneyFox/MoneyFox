using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using MoneyFox.Uwp.Views.Dialogs;
using System;
using Windows.UI.Xaml;

#nullable enable
namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCashFlowView
    {
        public StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel)DataContext;

        public override string Header => Strings.CashFlowStatisticTitle;

        public StatisticCashFlowView()
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
