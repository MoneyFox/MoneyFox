namespace MoneyFox.Ui.Views.Statistics.Selector;

public partial class StatisticSelectorPage
{
    public StatisticSelectorPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticSelectorViewModel>();
    }
}
