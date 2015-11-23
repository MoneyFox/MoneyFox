using Android.App;
using Android.Content;
using Android.Net;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Droid
{
    /// <summary>
    ///     Gives access to the features of google play on android.
    /// </summary>
    public class StoreFeatures : IStoreFeatures
    {
        /// <summary>
        ///     Open the app page in the playstore to rate the app. If the store page couldn't be opened,
        ///     the browser will be opened.
        /// </summary>
        public void RateApp()
        {
            string appPackageName = Application.Context.PackageName;

            try
            {
                var intent = new Intent(Intent.ActionView, Uri.Parse("market://details?id=" + appPackageName));
                // we need to add this, because the activity is in a new context.
                // Otherwise the runtime will block the execution and throw an exception
                intent.AddFlags(ActivityFlags.NewTask);

                Application.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                var intent = new Intent(Intent.ActionView, Uri.Parse("http://play.google.com/store/apps/details?id=" + appPackageName));
                // we need to add this, because the activity is in a new context.
                // Otherwise the runtime will block the execution and throw an exception
                intent.AddFlags(ActivityFlags.NewTask);

                Application.Context.StartActivity(intent);
            }
        }
    }
}