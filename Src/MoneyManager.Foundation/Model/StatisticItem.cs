using PropertyChanged;

namespace MoneyManager.Foundation.Model
{
    [ImplementPropertyChanged]
    public class StatisticItem
    {
        /// <summary>
        ///     Value used to group the items
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///     Value of this item
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        ///     Label to show in the chart
        /// </summary>
        public string Label { get; set; }
    }
}