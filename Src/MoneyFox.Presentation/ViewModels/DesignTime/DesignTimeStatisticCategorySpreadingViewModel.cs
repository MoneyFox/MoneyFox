using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Microcharts;
using MoneyFox.Application.Resources;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.ViewModels.Statistic;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeStatisticCategorySpreadingViewModel : IStatisticCategorySpreadingViewModel
    {
        public DesignTimeStatisticCategorySpreadingViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <inheritdoc />
        public LocalizedResources Resources { get; }

        public string Title => "I AM A MIGHTY TITLE";

        public DonutChart Chart => new DonutChart
        {
            Entries = new List<Entry>
            {
                new Entry(1234) {Label = "Essen"},
                new Entry(1465) {Label = "Bier"},
                new Entry(543) {Label = "Boooze"},
                new Entry(462) {Label = "Rent"},
                new Entry(1112) {Label = "Clothes"},
                new Entry(512) {Label = "Eating or so?"},
            }
        };

        public ObservableCollection<StatisticEntry> StatisticItems => new ObservableCollection<StatisticEntry>(new List<StatisticEntry>
        {
            new StatisticEntry(1234) {Label = "Essen"},
            new StatisticEntry(1465) {Label = "Bier"},
            new StatisticEntry(543) {Label = "Boooze"},
            new StatisticEntry(462) {Label = "Rent"},
            new StatisticEntry(1112) {Label = "Clothes"},
            new StatisticEntry(512) {Label = "Eating or so?"},
        });

        public AsyncCommand LoadedCommand { get; } = null;
    }
}
