using System;
using System.Diagnostics;
using Microsoft.AppCenter.Crashes;
using UIKit;

namespace MoneyFox.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            try
            {
                // if you want to use a different Application Delegate class from "AppDelegate"
                // you can specify it here.
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                Crashes.TrackError(ex);
            }
        }
    }
}
