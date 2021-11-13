using Foundation;
using System.Linq;
using UIKit;

[assembly: Preserve(typeof(Queryable), AllMembers = true)]

namespace MoneyFox.iOS
{
    public class Application
    {
        protected Application() { }

        // This is the main entry point of the application.
        private static void Main(string[] args) =>
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
    }
}