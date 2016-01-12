using AI.XamarinSDK.Abstractions;
using UIKit;

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
            ApplicationInsights.Setup("ac915a37-36f5-436a-b85b-5a5617838bc8");
            ApplicationInsights.Start();
        }
    }
}