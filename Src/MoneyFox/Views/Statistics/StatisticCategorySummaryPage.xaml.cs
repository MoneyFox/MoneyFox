namespace MoneyFox.Views.Statistics
{
    using System;
    using CommunityToolkit.Maui.Views;
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

        private void OpenFilterDialog(object sender, EventArgs e)
        {
            this.ShowPopup(new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate));
        }
    }

}
