namespace MoneyFox.ViewModels.Statistics
{

    using System.Collections.Generic;
    using CommunityToolkit.Mvvm.Input;

    public interface IStatisticSelectorViewModel
    {
        List<StatisticSelectorType> StatisticItems { get; }

        RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }
    }

}
