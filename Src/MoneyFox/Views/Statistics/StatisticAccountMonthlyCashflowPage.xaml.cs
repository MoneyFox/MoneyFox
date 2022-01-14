using MoneyFox.ViewModels.Statistics;
using MoneyFox.Views.Dialogs;
using System;
using Xamarin.Forms;

namespace MoneyFox.Views.Statistics
{
    public partial class StatisticAccountMonthlyCashFlowPage
    {
        private StatisticAccountMonthlyCashflowViewModel ViewModel =>
            (StatisticAccountMonthlyCashflowViewModel)BindingContext;

        public StatisticAccountMonthlyCashFlowPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatistcAccountMonthlyCashflowViewModel;
        }

        protected override void OnAppearing() => ViewModel.InitCommand.Execute(null);

        private async void OpenFilterDialog(object sender, EventArgs e)
            => await new DateSelectionPopup(ViewModel.StartDate, ViewModel.EndDate).ShowAsync();
    }
}