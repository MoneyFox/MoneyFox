namespace MoneyFox.Ui.Views.Statistics.Selector;

using Common.Navigation;

public partial class StatisticSelectorPage: IBindablePage
{
    public StatisticSelectorPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticSelectorViewModel>();
    }
}
