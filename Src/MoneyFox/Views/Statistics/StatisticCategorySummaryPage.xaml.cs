using MoneyFox.ViewModels.Statistics;
using MoneyFox.Views.Dialogs;
using System;

namespace MoneyFox.Views.Statistics
{
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
            => await new DateSelectionPopup(ViewModel.StartDate, ViewModel.EndDate).ShowAsync();
    }
}