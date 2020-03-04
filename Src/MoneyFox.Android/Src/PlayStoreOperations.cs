using Android.Content;
using Android.Net;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Droid.Src
{
    /// <summary>
    /// Gives access to the features of google play on android.
    /// </summary>
    public class PlayStoreOperations : IStoreOperations
    {
        private const string MARKET_URI = "market://details?id=";

        /// <summary>
        /// Open the app page in the play store to rate the app. If the store page couldn't be opened,     the browser
        /// will be opened.
        /// </summary>
        public void RateApp()
        {
            string appPackageName = Android.App.Application.Context.PackageName;

            try
            {
                var intent = new Intent(Intent.ActionView, Uri.Parse($"{MARKET_URI}{appPackageName}"));
                // we need to add this, because the activity is in a new context.
                // Otherwise the runtime will block the execution and throw an exception
                intent.AddFlags(ActivityFlags.NewTask);

                Android.App.Application.Context.StartActivity(intent);
            }
            catch(ActivityNotFoundException)
            {
                var intent = new Intent(Intent.ActionView,
                                        Uri.Parse($"http://play.google.com/store/apps/details?id={appPackageName}"));
                // we need to add this, because the activity is in a new context.
                // Otherwise the runtime will block the execution and throw an exception
                intent.AddFlags(ActivityFlags.NewTask);

                Android.App.Application.Context.StartActivity(intent);
            }
        }
    }
}
