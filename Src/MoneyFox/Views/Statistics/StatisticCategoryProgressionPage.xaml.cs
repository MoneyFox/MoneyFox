using MoneyFox.ViewModels.Statistics;
using MoneyFox.Views.Dialogs;
using System;
using Xamarin.Forms;

namespace MoneyFox.Views.Statistics
{
    public partial class StatisticCategoryProgressionPage : ContentPage
    {
        private StatisticCategoryProgressionViewModel ViewModel =>
            (StatisticCategoryProgressionViewModel)BindingContext;

        public StatisticCategoryProgressionPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategoryProgressionViewModel;
        }

        private async void OpenFilterDialog(object sender, EventArgs e)
            => await new DateSelectionPopup(ViewModel.StartDate, ViewModel.EndDate).ShowAsync();
    }
}