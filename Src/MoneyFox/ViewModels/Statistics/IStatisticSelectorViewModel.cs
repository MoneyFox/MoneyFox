namespace MoneyFox.ViewModels.Statistics;

using CommunityToolkit.Mvvm.Input;

public interface IStatisticSelectorViewModel
{
    List<StatisticSelectorTypeViewModel> StatisticItems { get; }

    RelayCommand<StatisticSelectorTypeViewModel> GoToStatisticCommand { get; }
}
