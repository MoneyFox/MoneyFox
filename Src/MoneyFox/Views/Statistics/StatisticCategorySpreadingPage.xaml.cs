namespace MoneyFox.Views.Statistics
{
    using System;
    using CommunityToolkit.Maui.Views;
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

        private void OpenFilterDialog(object sender, EventArgs e)
        {
            this.ShowPopup(new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate));
        }
    }

}
