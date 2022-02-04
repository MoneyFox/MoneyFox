using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Statistics;

namespace MoneyFox.Win.Pages.Statistics
{
    public sealed partial class StatisticSelectorView
    {
        public override string Header => Strings.SelectStatisticTitle;

        private StatisticSelectorViewModel ViewModel => (StatisticSelectorViewModel) DataContext;

        public StatisticSelectorView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticSelectorVm;
        }
    }
}