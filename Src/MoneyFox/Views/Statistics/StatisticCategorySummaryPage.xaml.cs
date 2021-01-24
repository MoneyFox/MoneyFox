using MoneyFox.Presentation.Dialogs;
using MoneyFox.ViewModels.Statistics;
using System;

namespace MoneyFox.Views.Statistics
{
    public partial class StatisticCategorySummaryPage
    {
        private StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)BindingContext;

        public StatisticCategorySummaryPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategorySummaryViewModel;
            ViewModel.LoadedCommand.Execute(null);
        }

        private static async void OpenFilterDialog(object sender, EventArgs e)
            => await new DateSelectionPopup().ShowAsync();
    }
}
