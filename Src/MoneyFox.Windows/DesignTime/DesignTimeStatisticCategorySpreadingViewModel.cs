using System.Collections.Generic;
using Microcharts;
using MoneyFox.Business.ViewModels.Interfaces;

namespace MoneyFox.Windows.DesignTime
{
    public class DesignTimeStatisticCategorySpreadingViewModel : IStatisticCategorySpreadingViewModel
    {
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
    }
}
