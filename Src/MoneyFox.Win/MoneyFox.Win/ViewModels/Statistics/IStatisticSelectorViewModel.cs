using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

namespace MoneyFox.Win.ViewModels.Statistics
{
    public interface IStatisticSelectorViewModel
    {
        RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }

        List<StatisticSelectorType> StatisticItems { get; }
    }
}