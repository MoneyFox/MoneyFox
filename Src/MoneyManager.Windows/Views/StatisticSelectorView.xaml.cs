using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views
{
    public sealed partial class StatisticSelectorView
    {
        public StatisticSelectorView()
        {
            InitializeComponent();

            DataContext = Mvx.Resolve<StatisticSelectorViewModel>();
        }
    }
}
