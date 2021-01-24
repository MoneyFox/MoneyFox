using MoneyFox.Presentation.Dialogs;
using MoneyFox.ViewModels.Statistics;
using System;
using Xamarin.Forms;

namespace MoneyFox.Views.Statistics
{
    public partial class StatisticAccountMonthlyCashflowPage : ContentPage
    {
        private StatisticAccountMonthlyCashflowViewModel ViewModel => (StatisticAccountMonthlyCashflowViewModel)BindingContext;

        public StatisticAccountMonthlyCashflowPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatistcAccountMonthlyCashflowViewModel;
        }

        protected override void OnAppearing() => ViewModel.InitCommand.Execute(null);

        private async void OpenFilterDialog(object sender, EventArgs e)
            => await new DateSelectionPopup(ViewModel.StartDate, ViewModel.EndDate).ShowAsync();
    }
}
