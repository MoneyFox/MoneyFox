using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class StatisticsView
    {
        public StatisticsView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<StatisticViewModel>();
        }
    }
}