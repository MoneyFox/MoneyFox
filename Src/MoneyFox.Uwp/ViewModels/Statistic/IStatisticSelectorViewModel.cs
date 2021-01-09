using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using System.Collections.Generic;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public interface IStatisticSelectorViewModel
    {
        RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }

        List<StatisticSelectorType> StatisticItems { get; }
    }
}
