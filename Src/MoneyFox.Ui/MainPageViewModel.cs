namespace MoneyFox.Ui;

using CommunityToolkit.Mvvm.ComponentModel;
using Views.Dashboard;
using Views.OverflowMenu;
using Views.Statistics.Selector;

public sealed class MainPageViewModel : ObservableObject
{
    private int selectedViewModelIndex;

    public MainPageViewModel(
        DashboardViewModel dashboardViewModel,
        StatisticSelectorViewModel statisticSelectorViewModel,
        OverflowMenuViewModel overflowMenuViewModel)
    {
        DashboardViewModel = dashboardViewModel;
        StatisticSelectorViewModel = statisticSelectorViewModel;
        OverflowMenuViewModel = overflowMenuViewModel;
    }

    public DashboardViewModel DashboardViewModel { get; }

    public StatisticSelectorViewModel StatisticSelectorViewModel { get; }

    public OverflowMenuViewModel OverflowMenuViewModel { get; }

    public int SelectedViewModelIndex
    {
        get => selectedViewModelIndex;
        set => SetProperty(field: ref selectedViewModelIndex, newValue: value);
    }
}
