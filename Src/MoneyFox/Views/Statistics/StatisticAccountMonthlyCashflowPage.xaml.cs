namespace MoneyFox.Views.Statistics
{
    using Dialogs;
    using System;
    using ViewModels.Statistics;

    public partial class StatisticAccountMonthlyCashFlowPage
    {
        public StatisticAccountMonthlyCashFlowPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatistcAccountMonthlyCashflowViewModel;
        }

        private StatisticAccountMonthlyCashflowViewModel ViewModel =>
            (StatisticAccountMonthlyCashflowViewModel)BindingContext;

        protected override void OnAppearing() => ViewModel.InitCommand.Execute(null);

        private async void OpenFilterDialog(object sender, EventArgs e)
            => await new DateSelectionPopup(ViewModel.StartDate, ViewModel.EndDate).ShowAsync();
    }
}