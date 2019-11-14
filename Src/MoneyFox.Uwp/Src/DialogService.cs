using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Interfaces;
using MoneyFox.Uwp.Views.Dialogs;

namespace MoneyFox.Uwp
{
    public class DialogService : IDialogService
    {
        private LoadingDialog loadingDialog;

        /// <summary>
        ///     Show a dialog with two buttons with customizable Texts. Returns the answer.
        /// </summary>
        /// <param name="title">Title for the dialog.</param>
        /// <param name="message">Text for the dialog.</param>
        /// <param name="positiveButtonText">Text for the yes button.</param>
        /// <param name="negativeButtonText">Text for the no button.</param>
        public async Task<bool> ShowConfirmMessageAsync(string title, string message, string positiveButtonText = null,
                                                   string negativeButtonText = null)
        {
            await HideLoadingDialog();

            var dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand(positiveButtonText ?? Strings.YesLabel));
            dialog.Commands.Add(new UICommand(negativeButtonText ?? Strings.NoLabel));

            IUICommand result = await dialog.ShowAsync();

            return result.Label == (positiveButtonText ?? Strings.YesLabel);
        }

        /// <summary>
        ///     Shows a dialog with title and message. Contains only an OK button.
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Text to display.</param>
        public async Task ShowMessage(string title, string message)
        {
            await HideLoadingDialog();

            var dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand(Strings.OkLabel));

            await dialog.ShowAsync();
        }

        /// <summary>
        ///     Shows a loading Dialog.
        /// </summary>
        public async Task ShowLoadingDialog(string message = null)
        {
            // Be sure no other dialog is open.
            await HideLoadingDialog();

            loadingDialog = new LoadingDialog {Text = message ?? Strings.LoadingLabel};
            await loadingDialog.ShowAsync();
        }

        /// <summary>
        ///     Hides the previously opened Loading Dialog.
        /// </summary>
        public Task HideLoadingDialog()
        {
            loadingDialog?.Hide();
            return Task.CompletedTask;
        }
    }
}
