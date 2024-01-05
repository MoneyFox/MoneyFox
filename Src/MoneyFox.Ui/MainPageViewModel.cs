namespace MoneyFox.Ui;

using CommunityToolkit.Mvvm.ComponentModel;
using Views.Dashboard;
using Views.OverflowMenu;
using Views.Statistics.Selector;

public sealed class MainPageViewModel(
    DashboardViewModel viewModel,
    StatisticSelectorViewModel statisticSelectorViewModel,
    OverflowMenuViewModel overflowMenuViewModel) : ObservableObject
{
    private int selectedViewModelIndex;

    public StatisticSelectorViewModel StatisticSelectorViewModel { get; } = statisticSelectorViewModel;

    public OverflowMenuViewModel OverflowMenuViewModel { get; } = overflowMenuViewModel;

    public DashboardViewModel DashboardViewModel
    {
        get => viewModel;
        set => SetProperty(field: ref viewModel, newValue: value);
    }

    public int SelectedViewModelIndex
    {
        get => selectedViewModelIndex;
        set => SetProperty(field: ref selectedViewModelIndex, newValue: value);
    }
}
