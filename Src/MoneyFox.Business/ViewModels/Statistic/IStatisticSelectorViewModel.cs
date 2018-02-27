using System.Collections.Generic;
using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Statistic
{
    public interface IStatisticSelectorViewModel
    {
        List<StatisticSelectorType> StatisticItems { get; }

        MvxAsyncCommand<StatisticSelectorType> GoToStatisticCommand { get; }
    }
}