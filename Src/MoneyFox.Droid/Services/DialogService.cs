using System;
using System.Threading.Tasks;
using Android.App;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using AndroidHUD;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Droid.Services
{
    public class DialogService : IDialogService 
    {
        protected Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        /// <summary>
        ///     Shows a dialog with title and message. Contains only an OK button.
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Text to display.</param>
        public Task ShowMessage(string title, string message)
        {
            // We have to hide the loading dialog first, otherwise it get's stuck.
            HideLoadingDialog();
            var tcs = new TaskCompletionSource<bool>();

            var builder = new AlertDialog.Builder(CurrentActivity);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.Show();

            return tcs.Task;
        }

        /// <summary>
        ///     Show a dialog with two buttons with customizable Texts. If no message is passed the dialog will have a Yes and No
        ///     Button
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Text to display.</param>
        /// <param name="positiveButtonText">Text for the yes button.</param>
        /// <param name="negativeButtonText">Text for the no button.</param>
        /// <param name="positivAction">Action who shall be executed on the positive button click.</param>
        /// <param name="negativAction">Action who shall be executed on the negative button click.</param>
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

        /// <summary>
        ///     Show a dialog with two buttons with customizable Texts. Returns the answer.
        /// </summary>
        /// <param name="title">Title for the dialog.</param>
        /// <param name="message">Text for the dialog.</param>
        /// <param name="positiveButtonText">Text for the yes button.</param>
        /// <param name="negativeButtonText">Text for the no button.</param>
        public Task<bool> ShowConfirmMessage(string title, string message, string positiveButtonText = null,
            string negativeButtonText = null)
        {
            // We have to hide the loading dialog first, otherwise it get's stuck.
            HideLoadingDialog();
            var tcs = new TaskCompletionSource<bool>();

            var builder = new AlertDialog.Builder(CurrentActivity);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetPositiveButton(Strings.YesLabel, (s, e) => tcs.SetResult(true));
            builder.SetNegativeButton(Strings.NoLabel, (s, e) => tcs.SetResult(false));
            builder.Show();

            return tcs.Task;
        }

        /// <summary>
        ///     Shows a loading dialog on the whole display.
        /// </summary>
        public void ShowLoadingDialog()
        {
            AndHUD.Shared.Show(CurrentActivity, Strings.LoadingLabel);
        }

        /// <summary>
        ///     Closes the Loading Dialog.
        /// </summary>
        public void HideLoadingDialog() 
        {
            AndHUD.Shared.Dismiss(CurrentActivity);
        }
    }
}