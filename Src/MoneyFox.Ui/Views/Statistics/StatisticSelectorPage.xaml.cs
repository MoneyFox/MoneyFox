namespace MoneyFox.Views.Statistics;

using Ui;
using ViewModels.Statistics;

public partial class StatisticSelectorPage
{
    public StatisticSelectorPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticSelectorViewModel>();
    }
}
