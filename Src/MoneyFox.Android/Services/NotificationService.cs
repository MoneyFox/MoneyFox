using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Views.InputMethods;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MoneyFox.Droid.Services
{
    public class NotificationService : INotificationService
    {
        protected Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public Task SendBasicNotification(string title, string message)
        {
            var tcs = new TaskCompletionSource<bool>();

            var view = CurrentActivity.Window.DecorView.FindViewById(Android.Resource.Id.Content);

            // Will close keyboard if open to make the snackbar visible
            if (view != null)
            {
                var imm = (InputMethodManager) CurrentActivity
                    .GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(view.WindowToken, 0);
            }

            Snackbar
                .Make(view,
                    message, Snackbar.LengthLong)
                .Show();

            return tcs.Task;
        }
    }
}