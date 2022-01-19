using System.Drawing;

namespace MoneyFox.Views.Statistics
{
    /// <summary>
    ///     Provides a set of default values for charts.
    ///     Those can be overwritten for a specific platform
    /// </summary>
    public static class ChartOptions
    {
        public static float Margin { get; set; } = 20;
        public static float LabelTextSize { get; set; } = 26f;
        public static Color BackgroundColor { get; set; } = Color.Transparent;
        public static string TypeFace { get; set; } = "Lobster-Regular";
    }
}