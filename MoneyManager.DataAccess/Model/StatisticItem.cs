using PropertyChanged;

namespace MoneyManager.DataAccess.Model
{
    [ImplementPropertyChanged]
    public class StatisticItem
    {
        public string Category { get; set; }

        public double Value { get; set; }
    }
}