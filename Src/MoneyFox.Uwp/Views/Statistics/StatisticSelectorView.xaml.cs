using MoneyFox.Application.Resources;

#nullable enable
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