using MoneyFox.Views.Dialogs;
using System;
using System.Threading.Tasks;

namespace MoneyFox.Services
{
    public class DialogService : IDialogService
    {
        private LoadingDialog? loadingDialog;

        /// <inheritdoc/>
        public async Task ShowLoadingDialogAsync(string? message = null)
        {
            if(loadingDialog != null)
            {
                await HideLoadingDialogAsync();
            }

            loadingDialog = new LoadingDialog();
            await loadingDialog.ShowAsync();
        }

        /// <inheritdoc/>
        public async Task HideLoadingDialogAsync()
        {
            if(loadingDialog == null)
                return;

            try
            {
                await loadingDialog.DismissAsync();
            }
            catch(IndexOutOfRangeException)
            {
                // catch and swallow out of range exceptions when dismissing dialogs.
            }
        }

        public async Task ShowMessageAsync(string title, string message)
        {
            var messageDialog = new MessageDialog(title, message);
            await messageDialog.ShowAsync();
        }

        public async Task<bool> ShowConfirmMessageAsync(string title, string message, string? positiveButtonText = null, string? negativeButtonText = null)
        {
            var confirmDialog = new ConfirmMessageDialog(title, message, positiveButtonText ?? "", negativeButtonText ?? "" );
            return await confirmDialog.ShowAsync();
        }
    }
}
