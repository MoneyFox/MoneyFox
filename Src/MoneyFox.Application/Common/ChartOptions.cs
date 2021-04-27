using SkiaSharp;

namespace MoneyFox.Application.Common
{
    /// <summary>
    ///     Provides a set of default values for charts.
    ///     Those can be overwritten for a specific platform
    /// </summary>
    public static class ChartOptions
    {
        public static float Margin { get; set; } = 20;
        public static float LabelTextSize { get; set; } = 26f;
        public static SKColor BackgroundColor { get; set; } = SKColors.Transparent;
        public static SKTypeface TypeFace { get; set; } = SKTypeface.FromFamilyName("Lobster-Regular");
    }
}
