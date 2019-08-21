using MoneyFox.Domain;

namespace MoneyFox.Presentation.Models
{
    /// <summary>
    ///     Represents a item for the selector to choose the statistic.
    /// </summary>
    public class StatisticSelectorType
    {
        /// <summary>
        ///     Name of the statistic
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Short description for the statistic
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Type of this item.
        /// </summary>
        public StatisticType Type { get; set; }
    }
}
