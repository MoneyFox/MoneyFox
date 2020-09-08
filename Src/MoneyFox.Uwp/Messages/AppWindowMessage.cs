using Windows.UI.WindowManagement;

namespace MoneyFox.Uwp.Messages
{
    public class AppWindowMessage
    {
        public AppWindowMessage(AppWindow appWindow)
        {
            AppWindow = appWindow;
        }

        public AppWindow AppWindow { get; }
    }
}
