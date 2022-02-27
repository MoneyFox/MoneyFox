using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Resources;
using MoneyFox.Views.Dialogs;
using System;
using System.Threading.Tasks;

namespace MoneyFox.Services
{
    public class DialogService : IDialogService
    {
        private LoadingDialog? loadingDialog;

        /// <inheritdoc />
        public async Task ShowLoadingDialogAsync(string? message = null)
        {
            if(loadingDialog != null)
            {
                await HideLoadingDialogAsync();
            }

            loadingDialog = new LoadingDialog();
            await loadingDialog.ShowAsync();
        }

        /// <inheritdoc />
        public async Task HideLoadingDialogAsync()
        {
            if(loadingDialog == null)
            {
                return;
            }

            try
            {
                await LoadingDialog.DismissAsync();
                loadingDialog = null;
            }
            catch(IndexOutOfRangeException)
            {
                // catch and swallow out of range exceptions when dismissing dialogs.
            }
        }

        public async Task ShowMessageAsync(string title, string message)
        {
            if(loadingDialog != null)
            {
                // Only 1 dialog can be open at a time. Close the Loading dialog sow the message can be displayed.
                await HideLoadingDialogAsync();
            }

            var messageDialog = new MessageDialog(title, message);
            await messageDialog.ShowAsync();
        }

        public async Task<bool> ShowConfirmMessageAsync(string title,
            string message,
            string? positiveButtonText = null,
            string? negativeButtonText = null)
        {
            if(loadingDialog != null)
            {
                // Only 1 dialog can be open at a time. Close the Loading dialog sow the message can be displayed.
                await HideLoadingDialogAsync();
            }

            var confirmDialog = new ConfirmMessageDialog(
                title,
                message,
                positiveButtonText ?? Strings.YesLabel,
                negativeButtonText ?? Strings.NoLabel);
            return await confirmDialog.ShowAsync();

        }
    }
}