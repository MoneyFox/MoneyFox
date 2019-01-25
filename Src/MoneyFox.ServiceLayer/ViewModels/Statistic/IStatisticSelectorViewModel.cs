using System.Collections.Generic;
using MoneyFox.Foundation.Models;
using MvvmCross.Commands;

namespace MoneyFox.ServiceLayer.ViewModels.Statistic
{
    public interface IStatisticSelectorViewModel : IBaseViewModel
    {
        List<StatisticSelectorType> StatisticItems { get; }

        MvxAsyncCommand<StatisticSelectorType> GoToStatisticCommand { get; }
    }
}