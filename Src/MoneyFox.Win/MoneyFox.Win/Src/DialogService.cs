using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Resources;
using MoneyFox.Win.Pages.Dialogs;
using System;
using System.Threading.Tasks;

namespace MoneyFox.Win
{
    public class DialogService : IDialogService
    {
        private LoadingDialog? loadingDialog;

        /// <summary>
        ///     Show a dialog with two buttons with customizable Texts. Returns the answer.
        /// </summary>
        /// <param name="title">Title for the dialog.</param>
        /// <param name="message">Text for the dialog.</param>
        /// <param name="positiveButtonText">Text for the yes button.</param>
        /// <param name="negativeButtonText">Text for the no button.</param>
        public async Task<bool> ShowConfirmMessageAsync(string title,
            string message,
            string? positiveButtonText = null,
            string? negativeButtonText = null)
        {
            var dialog = new ContentDialog { XamlRoot = MainWindow.RootFrame.XamlRoot, Title = title, Content = message };
            dialog.PrimaryButtonText = positiveButtonText ?? Strings.YesLabel;
            dialog.SecondaryButtonText = negativeButtonText ?? Strings.NoLabel;

            ContentDialogResult result = await dialog.ShowAsync();

            return result == ContentDialogResult.Primary;
        }

        /// <summary>
        ///     Shows a dialog with title and message. Contains only an OK button.
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Text to display.</param>
        public async Task ShowMessageAsync(string title, string message)
        {
            var dialog = new ContentDialog { XamlRoot = MainWindow.RootFrame.XamlRoot, Title = title, Content = message };
            dialog.PrimaryButtonText = Strings.OkLabel;

            await dialog.ShowAsync();
        }

        /// <summary>
        ///     Shows a loading Dialog.
        /// </summary>
        public Task ShowLoadingDialogAsync(string? message = null)
        {
            loadingDialog = new LoadingDialog { Text = message ?? Strings.LoadingLabel };

            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            dispatcherQueue.TryEnqueue(async () =>
            {
                await loadingDialog.ShowAsync();
            });

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Hides the previously opened Loading Dialog.
        /// </summary>
        public Task HideLoadingDialogAsync()
        {
            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            dispatcherQueue.TryEnqueue(() =>
            {
                loadingDialog?.Hide();
            });
            return Task.CompletedTask;
        }
    }
}