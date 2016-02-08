using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
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