using CommunityToolkit.Mvvm.Input;
using MoneyFox.Uwp.ViewModels.Statistics;
using System.Collections.Generic;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    public interface IStatisticSelectorViewModel
    {
        RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }

        List<StatisticSelectorType> StatisticItems { get; }
    }
}