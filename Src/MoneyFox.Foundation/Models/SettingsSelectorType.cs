namespace MoneyFox.Foundation.Models
{
    /// <summary>
    ///     Represents a item for the selector to choose the settings.
    /// </summary>
    public class SettingsSelectorType
    {
        /// <summary>
        ///     Name of the statistic
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Code for the Icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        ///     Short description for the statistic
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Type of this item.
        /// </summary>
        public SettingsType Type { get; set; }
    }
}
