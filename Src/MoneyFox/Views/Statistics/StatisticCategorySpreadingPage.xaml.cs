namespace MoneyFox.Views.Statistics
{

    using System;
    using Dialogs;
    using ViewModels.Statistics;

    public partial class StatisticCategorySpreadingPage
    {
        public StatisticCategorySpreadingPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategorySpreadingViewModel;
            ViewModel.LoadedCommand.Execute(null);
        }

        private StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)BindingContext;

        private async void OpenFilterDialog(object sender, EventArgs e)
        {
            await new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate).ShowAsync();
        }
    }

}
