using CommunityToolkit.Mvvm.Input;
using MoneyFox.Core.Queries.Statistics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MoneyFox.Win.ViewModels.DesignTime
{
    public class DesignTimeStatisticCategorySpreadingViewModel : IStatisticCategorySpreadingViewModel
    {
        public static string Title => "I AM A MIGHTY TITLE";

        public AsyncRelayCommand LoadedCommand { get; } = null!;

        public ObservableCollection<StatisticEntry> StatisticItems
            => new ObservableCollection<StatisticEntry>(
                new List<StatisticEntry>
                {
                    new StatisticEntry(1234) { Label = "Essen" },
                    new StatisticEntry(1465) { Label = "Bier" },
                    new StatisticEntry(543) { Label = "Boooze" },
                    new StatisticEntry(462) { Label = "Rent" },
                    new StatisticEntry(1112) { Label = "Clothes" },
                    new StatisticEntry(512) { Label = "Eating or so?" }
                });
    }
}