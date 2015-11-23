using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels.Statistics;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class CategorySummaryUserControl
    {
        public CategorySummaryUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<StatisticViewModel>();
        }
    }
}