using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views {
    public sealed partial class StatisticSelectorView {
        public StatisticSelectorView() {
            InitializeComponent();

            DataContext = Mvx.Resolve<StatisticSelectorViewModel>();
        }
    }
}