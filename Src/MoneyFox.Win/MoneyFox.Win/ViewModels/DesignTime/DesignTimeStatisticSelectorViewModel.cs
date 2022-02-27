namespace MoneyFox.Win.ViewModels.DesignTime;

using CommunityToolkit.Mvvm.Input;
using Core;
using Core.Resources;
using Statistics;
using System.Collections.Generic;

public class DesignTimeStatisticSelectorViewModel : IStatisticSelectorViewModel
{
    public List<StatisticSelectorType> StatisticItems => new()
    {
        new()
        {
            Name = Strings.CashflowLabel,
            Description = Strings.CashflowDescription,
            Type = StatisticType.Cashflow
        },
        new()
        {
            Name = Strings.CategorySpreadingLabel,
            Description = Strings.CategorieSpreadingDescription,
            Type = StatisticType.CategorySpreading
        },
        new()
        {
            Name = Strings.CategorySummaryLabel,
            Description = Strings.CategorySummaryDescription,
            Type = StatisticType.CategorySummary
        }
    };

    public RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; } = null!;
}