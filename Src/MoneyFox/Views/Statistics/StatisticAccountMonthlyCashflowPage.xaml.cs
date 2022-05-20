namespace MoneyFox.Views.Statistics
{

    using System;
    using CommunityToolkit.Maui.Views;
    using Dialogs;
    using ViewModels.Statistics;

    public partial class StatisticAccountMonthlyCashFlowPage
    {
        public StatisticAccountMonthlyCashFlowPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticAccountMonthlyCashFlowViewModel;
        }

        private StatisticAccountMonthlyCashFlowViewModel ViewModel => (StatisticAccountMonthlyCashFlowViewModel)BindingContext;

        protected override void OnAppearing()
        {
            ViewModel.InitCommand.Execute(null);
        }

        private void OpenFilterDialog(object sender, EventArgs e)
        {
            this.ShowPopup(new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate));
        }
    }

}
