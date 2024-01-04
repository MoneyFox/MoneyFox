namespace MoneyFox.Ui.Common.Extensions;

public static class DeviceIdiomExtensions
{
    public static bool UseDesktopPage(this DeviceIdiom deviceIdiom)
    {
        return deviceIdiom == DeviceIdiom.Desktop || deviceIdiom == DeviceIdiom.Tablet || deviceIdiom == DeviceIdiom.TV;
    }
}
