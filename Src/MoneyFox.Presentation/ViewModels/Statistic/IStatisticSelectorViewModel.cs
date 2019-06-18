using System.Collections.Generic;
using MoneyFox.Foundation.Models;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross.Commands;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    public interface IStatisticSelectorViewModel : IBaseViewModel
    {
        List<StatisticSelectorType> StatisticItems { get; }

        MvxAsyncCommand<StatisticSelectorType> GoToStatisticCommand { get; }
    }
}