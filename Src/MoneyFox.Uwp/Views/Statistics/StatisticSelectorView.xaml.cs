using MoneyFox.Application.Resources;

namespace MoneyFox.Uwp.Views.Statistics
{
    public sealed partial class StatisticSelectorView : BaseView
    {
        public override string Header => Strings.SelectStatisticTitle;

        public override bool ShowHeader => true;

        public StatisticSelectorView()
        {
            InitializeComponent();
        }
    }
}
