namespace MoneyFox.Win.Pages.Statistics;

using Core.Resources;
using ViewModels.Statistics;

public sealed partial class StatisticSelectorPage
{
    public StatisticSelectorPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.StatisticSelectorVm;
    }

    public override string Header => Strings.SelectStatisticTitle;

    private StatisticSelectorViewModel ViewModel => (StatisticSelectorViewModel)DataContext;
}
