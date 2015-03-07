#region

using PropertyChanged;

#endregion

namespace MoneyManager.Foundation.Model {
    [ImplementPropertyChanged]
    public class StatisticItem {
        public string Category { get; set; }

        public double Value { get; set; }

        public string Label { get; set; }
    }
}