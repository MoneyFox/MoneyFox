using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;
using Windows.UI.Xaml;

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
