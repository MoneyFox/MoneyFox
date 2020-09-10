using MoneyFox.Uwp.ViewModels;
using MoneyFox.Ui.Shared.Groups;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
