using MoneyFox.ViewModels.Statistics;
using Xamarin.Forms;

namespace MoneyFox.Views.Statistics
{
    public partial class StatistcAccountMonthlyCashflowPage : ContentPage
    {
        private StatisticAccountMonthlyCashflowViewModel ViewModel => (StatisticAccountMonthlyCashflowViewModel)BindingContext;

        public StatistcAccountMonthlyCashflowPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatistcAccountMonthlyCashflowViewModel;
        }

        protected override void OnAppearing() => ViewModel.InitCommand.Execute(null);
    }
}