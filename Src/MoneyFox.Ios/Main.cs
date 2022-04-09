using System.Linq;
using Foundation;

[assembly: Preserve(typeof(Queryable), AllMembers = true)]

namespace MoneyFox.iOS
{

    using UIKit;

    public class Application
    {
        protected Application() { }

        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            UIApplication.Main(args: args, principalClass: null, delegateClass: typeof(AppDelegate));
        }
    }

}
