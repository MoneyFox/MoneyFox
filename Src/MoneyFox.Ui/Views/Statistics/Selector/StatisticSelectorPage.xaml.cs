namespace MoneyFox.Ui.Views.Statistics;

using MoneyFox.Ui.Views.Statistics.Selector;
using ViewModels.Statistics;

public partial class StatisticSelectorPage
{
    public StatisticSelectorPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticSelectorViewModel>();
    }
}
