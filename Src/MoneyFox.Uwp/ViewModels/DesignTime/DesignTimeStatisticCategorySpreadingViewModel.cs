using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoneyFox.Application.Statistics;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Uwp.ViewModels.Statistic;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeStatisticCategorySpreadingViewModel : IStatisticCategorySpreadingViewModel
    {
        public string Title => "I AM A MIGHTY TITLE";

        public ObservableCollection<StatisticEntry> StatisticItems => new ObservableCollection<StatisticEntry>(new List<StatisticEntry>
                                                                                                               {
                                                                                                                   new StatisticEntry(1234)
                                                                                                                   {Label = "Essen"},
                                                                                                                   new StatisticEntry(1465)
                                                                                                                   {Label = "Bier"},
                                                                                                                   new StatisticEntry(543)
                                                                                                                   {Label = "Boooze"},
                                                                                                                   new StatisticEntry(462)
                                                                                                                   {Label = "Rent"},
                                                                                                                   new StatisticEntry(1112)
                                                                                                                   {Label = "Clothes"},
                                                                                                                   new StatisticEntry(512)
                                                                                                                   {Label = "Eating or so?"}
                                                                                                               });

        public AsyncCommand LoadedCommand { get; }
    }
}
