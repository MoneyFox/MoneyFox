#nullable enable
using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels.Statistics;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticCategoryProgressionView
    {
        public override string Header => Strings.MonthlyCashflowTitle;

        public StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)DataContext;

        public StatisticCategoryProgressionView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCategoryProgressionVm;
        }
    }
}