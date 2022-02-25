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
        private static void Main(string[] args)
        {
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}