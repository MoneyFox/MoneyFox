namespace MoneyFox.Views.Statistics
{

    using System;
    using Dialogs;
    using ViewModels.Statistics;

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
        {
            await new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate).ShowAsync();
        }
    }

}
