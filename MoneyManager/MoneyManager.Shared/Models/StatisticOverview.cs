using PropertyChanged;

namespace MoneyManager.Models
{
    [ImplementPropertyChanged]
    public class StatisticItem
    {
        public string Category { get; set; }

        public double Value { get; set; }
    }
}