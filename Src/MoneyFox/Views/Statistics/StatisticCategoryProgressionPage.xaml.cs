namespace MoneyFox.Views.Statistics
{
    using Dialogs;
    using System;
    using ViewModels.Statistics;
    using Xamarin.Forms;

    public partial class StatisticCategoryProgressionPage : ContentPage
    {
        public StatisticCategoryProgressionPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategoryProgressionViewModel;
        }

        private StatisticCategoryProgressionViewModel ViewModel =>
            (StatisticCategoryProgressionViewModel)BindingContext;

        private async void OpenFilterDialog(object sender, EventArgs e)
            => await new DateSelectionPopup(ViewModel.StartDate, ViewModel.EndDate).ShowAsync();
    }
}