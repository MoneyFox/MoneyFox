namespace MoneyFox.Ui.Controls;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

public class ModalContentPage : ContentPage
{
    public ModalContentPage()
    {
        On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.Automatic);
    }
}
