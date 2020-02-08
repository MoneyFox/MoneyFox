using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microcharts;
using MoneyFox.Application.Statistics;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Uwp.ViewModels.Statistic;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeStatisticCashFlowViewModel : IStatisticCashFlowViewModel
    {
        public string Title => "I AM A MIGHTY TITLE";

        public BarChart Chart => new BarChart
        {
            Entries = new List<Entry>
            {
                new Entry(1234) {Label = "Expense"},
                new Entry(1465) {Label = "Income"},
                new Entry(543) {Label = "Revenue"}
            }
        };

        public ObservableCollection<StatisticEntry> StatisticItems => new ObservableCollection<StatisticEntry>(new List<StatisticEntry>
        {
            new StatisticEntry(1234) {Label = "Expense"},
            new StatisticEntry(1465) {Label = "Income"},
            new StatisticEntry(543) {Label = "Revenue"}
        });

        public AsyncCommand LoadedCommand { get; }
    }
}
