using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;

namespace MoneyFox.Uwp.Views.Statistics.StatisticCategorySummary
{
    public partial class PaymentListUserControl
    {
        public PaymentListUserControl()
        {
            InitializeComponent();
        }

        public CategoryOverviewViewModel ViewModel => (CategoryOverviewViewModel)DataContext;
    }
}
