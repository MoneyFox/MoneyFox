namespace MoneyFox.Ui.Views.Statistics.Selector;

public partial class StatisticSelectorView
{
    public StatisticSelectorView()
    {
        InitializeComponent();
    }

    public StatisticSelectorViewModel ViewModel => (StatisticSelectorViewModel)BindingContext;
}
