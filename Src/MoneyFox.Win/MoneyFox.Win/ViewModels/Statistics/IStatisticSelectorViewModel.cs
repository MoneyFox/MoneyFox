namespace MoneyFox.Win.ViewModels.Statistics;

using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

public interface IStatisticSelectorViewModel
{
    RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }

    List<StatisticSelectorType> StatisticItems { get; }
}