namespace MoneyFox.Droid.Converters
{
    /// <summary>
    ///     Converts the OxyColor to MvxColor for binding in Android.
    /// </summary>
    public class OxyPlotColorConverter : MvxColorValueConverter<OxyColor>
    {
        protected override MvxColor Convert(OxyColor value, object parameter, CultureInfo culture)
            => new MvxColor(value.R, value.G, value.B, value.A);
    }
}