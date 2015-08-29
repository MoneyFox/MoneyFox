using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

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