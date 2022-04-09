namespace MoneyFox.Win.ViewModels.Statistics;

using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;

public interface IStatisticSelectorViewModel
{
    RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }

    List<StatisticSelectorType> StatisticItems { get; }
}
