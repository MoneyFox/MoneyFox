using System.Collections.Generic;
using MoneyFox.Foundation.Models;
using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels.Statistic
{
    public interface IStatisticSelectorViewModel : IBaseViewModel
    {
        List<StatisticSelectorType> StatisticItems { get; }

        MvxAsyncCommand<StatisticSelectorType> GoToStatisticCommand { get; }
    }
}