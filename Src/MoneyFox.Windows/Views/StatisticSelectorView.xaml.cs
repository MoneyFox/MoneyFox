using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;

namespace MoneyFox.Windows.Views
{
    public sealed partial class StatisticSelectorView
    {
        public StatisticSelectorView()
        {
            InitializeComponent();

            DataContext = ServiceLocator.Current.GetInstance<StatisticSelectorViewModel>();
        }
    }
}