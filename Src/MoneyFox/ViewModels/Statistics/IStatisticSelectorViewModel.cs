namespace MoneyFox.ViewModels.Statistics
{
    using CommunityToolkit.Mvvm.Input;
    using System.Collections.Generic;

    public interface IStatisticSelectorViewModel
    {
        List<StatisticSelectorType> StatisticItems { get; }

        RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }
    }
}