using MoneyFox.Application.Statistics;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Uwp.ViewModels.Statistic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    [SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "Not needed in design time")]
    public class DesignTimeStatisticCashFlowViewModel : IStatisticCashFlowViewModel
    {
        public string Title => "I AM A MIGHTY TITLE";

        public ObservableCollection<StatisticEntry> StatisticItems
                                                    => new ObservableCollection<StatisticEntry>(new List<StatisticEntry>
                                                    {
                                                        new StatisticEntry(1234) { Label = "Expense" },
                                                        new StatisticEntry(1465) { Label = "Income" },
                                                        new StatisticEntry(543) { Label = "Revenue" }
                                                    });

        public AsyncCommand LoadedCommand { get; } = null!;
    }
}
