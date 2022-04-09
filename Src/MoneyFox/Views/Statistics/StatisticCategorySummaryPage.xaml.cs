namespace MoneyFox.Views.Statistics
{

    using System;
    using Dialogs;
    using ViewModels.Statistics;

    public partial class StatisticCategorySummaryPage
    {
        public StatisticCategorySummaryPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategorySummaryViewModel;
            ViewModel.LoadedCommand.Execute(null);
        }

        private StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)BindingContext;

        private async void OpenFilterDialog(object sender, EventArgs e)
        {
            await new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate).ShowAsync();
        }
    }

}
