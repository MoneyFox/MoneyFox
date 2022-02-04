using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Statistics;

namespace MoneyFox.Win.Pages.Statistics
{
    public sealed partial class StatisticSelectorPage
    {
        public override string Header => Strings.SelectStatisticTitle;

        private StatisticSelectorViewModel ViewModel => (StatisticSelectorViewModel) DataContext;

        public StatisticSelectorPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticSelectorVm;
        }
    }
}