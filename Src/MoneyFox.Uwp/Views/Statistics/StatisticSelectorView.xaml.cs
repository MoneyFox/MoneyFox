#nullable enable
using MoneyFox.Application.Resources;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticSelectorView
    {
        public override string Header => Strings.SelectStatisticTitle;

        public StatisticSelectorView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticSelectorVm;
        }
    }
}