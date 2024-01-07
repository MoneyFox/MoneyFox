// ReSharper disable once CheckNamespace
namespace MoneyFox.Ui;

using UIKit;

#pragma warning disable S1118
public class Program
#pragma warning restore S1118
{
    // This is the main entry point of the application.
    private static void Main(string[] args)
    {
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args: args, principalClass: null, delegateClass: typeof(AppDelegate));
    }
}
