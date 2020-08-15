using GalaSoft.MvvmLight.Command;
using MoneyFox.Presentation.Models;
using System.Collections.Generic;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public interface IStatisticSelectorViewModel
    {
        List<StatisticSelectorType> StatisticItems { get; }

        RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }
    }
}
