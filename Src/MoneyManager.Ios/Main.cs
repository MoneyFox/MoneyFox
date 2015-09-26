using UIKit;
using Xamarin;

namespace MoneyManager.Ios
{
    public class Application
    {
        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            InitializeAppInsights();

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }

        private static void InitializeAppInsights()
        {
            var insightKey = "599ff6bfdc79368ff3d5f5629a57c995fe93352e";

#if DEBUG
            insightKey = Insights.DebugModeKey;
#endif
            if (!Insights.IsInitialized)
            {
                Insights.Initialize(insightKey);
            }
        }
    }
}