namespace MoneyFox.Views.Statistics
{
    using System;
    using CommunityToolkit.Maui.Views;
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

        private void OpenFilterDialog(object sender, EventArgs e)
        {
            this.ShowPopup(new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate));
        }
    }

}
