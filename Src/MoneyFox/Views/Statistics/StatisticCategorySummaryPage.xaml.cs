namespace MoneyFox.Views.Statistics
{

    using System;
    using Popups;
    using ViewModels.Statistics;
    using Xamarin.CommunityToolkit.Extensions;
    using Xamarin.Forms;

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
            var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
            Shell.Current.ShowPopup(popup);
        }
    }

}
