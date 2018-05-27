using System;
using System.Threading.Tasks;
using Android.App;
using AndroidHUD;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MoneyFox.Droid.Services
{
    /// <inheritdoc />
    public class DialogService : IDialogService 
    {
        private Activity currentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
        
        /// <inheritdoc />
        public Task ShowMessage(string title, string message)
        {
            // We have to hide the loading dialog first, otherwise it get's stuck.
            HideLoadingDialog();

            var builder = new AlertDialog.Builder(currentActivity);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.Show();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task ShowConfirmMessage(string title, string message, Action positivAction,
            string positiveButtonText = null, string negativeButtonText = null, Action negativAction = null)
        {
            var isPositiveAnswer = await ShowConfirmMessage(title, message, positiveButtonText, negativeButtonText);

            if (isPositiveAnswer)
            {
                positivAction();
            }
            else
            {
                negativAction?.Invoke();
            }
        }

        /// <inheritdoc />
        public Task<bool> ShowConfirmMessage(string title, string message, string positiveButtonText = null,
            string negativeButtonText = null)
        {
            // We have to hide the loading dialog first, otherwise it get's stuck.
            HideLoadingDialog();
            var tcs = new TaskCompletionSource<bool>();

            var builder = new AlertDialog.Builder(currentActivity);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetPositiveButton(Strings.YesLabel, (s, e) => tcs.SetResult(true));
            builder.SetNegativeButton(Strings.NoLabel, (s, e) => tcs.SetResult(false));
            builder.Show();

            return tcs.Task;
        }

        /// <inheritdoc />
        public void ShowLoadingDialog()
        {
            AndHUD.Shared.Show(currentActivity, Strings.LoadingLabel);
        }

        /// <inheritdoc />
        public void HideLoadingDialog() 
        {
            AndHUD.Shared.Dismiss(currentActivity);
        }
    }
}