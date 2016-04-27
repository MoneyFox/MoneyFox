using System.Threading.Tasks;
using MoneyFox.Shared.Interfaces;
using Android.Support.Design.Widget;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform;
using Android.App;
using Android.Views.InputMethods;

namespace MoneyFox.Droid.Src
{
    public class NotificationService : INotificationService
    {
        protected Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public Task SendBasicNotification(string title, string message)
        {
            var tcs = new TaskCompletionSource<bool>();

            var view = CurrentActivity.Window.DecorView.FindViewById(Android.Resource.Id.Content);
            
            // Will close keyboard if open to make the snackbar visible
            if(view != null) {
                InputMethodManager imm = (InputMethodManager)CurrentActivity
                    .GetSystemService(Android.Content.Context.InputMethodService);
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