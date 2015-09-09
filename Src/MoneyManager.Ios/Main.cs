using UIKit;
using Xamarin;

namespace MoneyManager.Ios
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            InitializeAppInsights();

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }

        private static void InitializeAppInsights()
        {
            var insightKey = "e5c4ac56bb1ca47559bc8d4973d0a8c4d78c7648";

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