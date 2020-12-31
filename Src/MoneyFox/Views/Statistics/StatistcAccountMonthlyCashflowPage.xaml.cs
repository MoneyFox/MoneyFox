using MoneyFox.ViewModels.Statistics;
using Xamarin.Forms;

namespace MoneyFox.Views.Statistics
{
    public partial class StatistcAccountMonthlyCashflowPage : ContentPage
    {
        private StatistcAccountMonthlyCashflowViewModel ViewModel => (StatistcAccountMonthlyCashflowViewModel)BindingContext;

        public StatistcAccountMonthlyCashflowPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatistcAccountMonthlyCashflowViewModel;

            ViewModel.InitCommand.Execute(null);
        }
    }
}