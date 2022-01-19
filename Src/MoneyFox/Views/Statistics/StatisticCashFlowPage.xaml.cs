using MoneyFox.ViewModels.Statistics;
using MoneyFox.Views.Dialogs;
using System;

namespace MoneyFox.Views.Statistics
{
    public partial class StatisticCashFlowPage
    {
        public StatisticCashFlowPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCashFlowViewModel;

            ViewModel.LoadedCommand.Execute(null);
        }

        private StatisticCashFlowViewModel ViewModel => (StatisticCashFlowViewModel)BindingContext;

        private async void OpenFilterDialog(object sender, EventArgs e)
            => await new DateSelectionPopup(ViewModel.StartDate, ViewModel.EndDate).ShowAsync();
    }
}