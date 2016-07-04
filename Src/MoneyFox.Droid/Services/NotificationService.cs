using MoneyFox.Shared.Interfaces;

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