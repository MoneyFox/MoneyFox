namespace MoneyFox.Views.Statistics
{

    using System;
    using Popups;
    using ViewModels.Statistics;
    using Xamarin.CommunityToolkit.Extensions;
    using Xamarin.Forms;

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
            var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
            Shell.Current.ShowPopup(popup);
        }
    }

}
