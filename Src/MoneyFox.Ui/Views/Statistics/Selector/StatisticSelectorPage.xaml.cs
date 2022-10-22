namespace MoneyFox.Ui.Views.Statistics;

using Selector;

public partial class StatisticSelectorPage
{
    public StatisticSelectorPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticSelectorViewModel>();
    }
}

