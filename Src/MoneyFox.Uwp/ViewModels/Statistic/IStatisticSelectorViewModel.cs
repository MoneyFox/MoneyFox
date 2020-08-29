using GalaSoft.MvvmLight.Command;
using MoneyFox.Uwp.ViewModels.Statistic;
using System.Collections.Generic;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public interface IStatisticSelectorViewModel
    {
        RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }
        List<StatisticSelectorType> StatisticItems { get; }
    }
}