using MoneyFox.ViewModels.Statistics;
using MoneyFox.Views.Dialogs;
using System;
using Xamarin.Forms;

namespace MoneyFox.Views.Statistics
{
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