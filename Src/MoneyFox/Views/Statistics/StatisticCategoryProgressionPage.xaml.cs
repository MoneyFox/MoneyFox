namespace MoneyFox.Views.Statistics
{

    using System;
    using Dialogs;
    using ViewModels.Statistics;

    public partial class StatisticCategoryProgressionPage : ContentPage
    {
        public StatisticCategoryProgressionPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategoryProgressionViewModel;
        }

        private StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)BindingContext;

        private async void OpenFilterDialog(object sender, EventArgs e)
        {
            await new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate).ShowAsync();
        }
    }

}
