namespace MoneyFox.Views.Statistics
{
    using System;
    using CommunityToolkit.Maui.Views;
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

        private void OpenFilterDialog(object sender, EventArgs e)
        {
            this.ShowPopup(new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate));
        }
    }

}
